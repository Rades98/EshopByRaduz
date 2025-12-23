using Database.Contracts;
using InOutbox.Orchestrator.Repos;
using Mediator.Request.Command;
using Stock.Domain.StockItems;

namespace Stock.App.StockItems.AssignStockItemsToOrder
{
    public sealed record AssignStockItemsToOrderCommand(Guid CheckoutReference, Guid OrderReference) : ICommand<AssignStockItemsToOrderResponse>
    {
        internal sealed class AssignStockItemsToOrderCommandHandler(
            IStockItemLookup stockLookup,
            IStockItemRepo repo,
            IOutboxRepo outboxRepo,
            IUnitOfWork uow)
            : ICommandHandler<AssignStockItemsToOrderCommand, AssignStockItemsToOrderResponse>
        {
            public async Task<AssignStockItemsToOrderResponse> Handle(AssignStockItemsToOrderCommand request, CancellationToken cancellationToken)
            {
                await uow.BeginTransactionAsync(cancellationToken);

                try
                {
                    var stockItemIds = await stockLookup.FindByCheckoutReference(request.CheckoutReference, cancellationToken);

                    var results = new List<(StockItemAggregate? Item, bool Failed)>();

                    foreach (var stockItemId in stockItemIds)
                    {
                        var stockItem = await repo.GetAsync(stockItemId, cancellationToken);

                        if (stockItem is null)
                        {
                            results.Add(new(null, true));

                            continue;
                        }

                        var res = stockItem.TryAssignUnitsToOrder(
                            request.OrderReference,
                            request.CheckoutReference
                        );

                        await repo.SaveAsync(stockItem, cancellationToken);

                        results.Add(new(stockItem, !res.IsSuccess));
                    }

                    if (results.Any(x => x.Failed))
                    {
                        await uow.RollbackTransactionAsync(cancellationToken);
                        return new(false);
                    }

                    var validResults = results.Where(x => x.Item is not null).ToList();

                    await outboxRepo.AddRangeAsync([.. validResults.SelectMany(x => x.Item!.DomainEvents)], cancellationToken);

                    var orderItems = new List<AssignStockItemToOrderResponse>();

                    foreach (var (Item, Failed) in validResults)
                    {
                        Item!.ClearDomainEvents();
                        var serials = Item!.GetAssignedToOrder(request.OrderReference).Select(x => x.SerialNumber.Value).ToList().AsReadOnly();
                        orderItems.Add(new(Item.Sku, Item.VariantId, serials));
                    }

                    await uow.CommitTransactionAsync(cancellationToken);
                    return new(true, orderItems.AsReadOnly());

                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(cancellationToken);
                    return new(false);
                }
            }
        }
    }
}
