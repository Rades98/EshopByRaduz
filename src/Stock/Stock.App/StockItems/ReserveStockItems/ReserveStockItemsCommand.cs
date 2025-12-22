using InOutbox.Orchestrator;
using Mediator.Request.Command;
using Stock.App.Common;
using Stock.Domain.StockItems;

namespace Stock.App.StockItems.ReserveStockItems
{
    public sealed record ReserveStockItemsCommand(ReserveStockItemsRequest Request) : ICommand<bool>
    {
        internal sealed class ReserveStockItemsCommandHandler(
            IStockItemLookup stockLookup,
            IStockItemRepo repo,
            IOutboxRepo outboxRepo,
            IUnitOfWork uow)
            : ICommandHandler<ReserveStockItemsCommand, bool>
        {
            public async Task<bool> Handle(ReserveStockItemsCommand request, CancellationToken cancellationToken)
            {
                await uow.BeginTransactionAsync(cancellationToken);

                try
                {
                    var results = new List<(StockItemAggregate? Item, bool Failed)>();

                    foreach (var item in request.Request.Items)
                    {
                        var stockItemId = await stockLookup.FindBySkuAndVariant(item.Sku, item.VariantId, cancellationToken);
                        var stockItem = await repo.GetAsync(stockItemId, cancellationToken);

                        if (stockItem is null)
                        {
                            results.Add(new(null, true));

                            continue;
                        }

                        var res = stockItem.TryLockUnits(
                            item.Amount,
                            DateTime.UtcNow.AddMinutes(10),
                            request.Request.CheckoutReference
                        );

                        await repo.SaveAsync(stockItem, cancellationToken);

                        results.Add(new(stockItem, !res));
                    }

                    if (results.Any(x => x.Failed))
                    {
                        await uow.RollbackTransactionAsync(cancellationToken);
                        return false;
                    }

                    var validResults = results.Where(x => x.Item is not null).ToList();

                    await outboxRepo.AddRangeAsync([.. validResults.SelectMany(x => x.Item!.DomainEvents)], cancellationToken);

                    foreach (var (Item, Failed) in validResults)
                    {
                        Item!.ClearDomainEvents();
                    }

                    await uow.CommitTransactionAsync(cancellationToken);
                    return true;

                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}
