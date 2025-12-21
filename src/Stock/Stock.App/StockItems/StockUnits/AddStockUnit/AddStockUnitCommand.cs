using Mediator.Request.Command;
using Stock.App.Common;
using Stock.App.Common.Outbox;

namespace Stock.App.StockItems.StockUnits.AddStockUnit
{
    public sealed record AddStockUnitCommand(string SerialNumber, string Sku, string VariantId, Guid WarehouseId) : ICommand<AddStockUnitCommandResult>
    {
        internal sealed class AddStockUnitCommandHandler(
            IStockItemLookup stockLookup,
            IStockItemRepo repo,
            IOutboxRepo outboxRepo,
            IUnitOfWork uow)
            : ICommandHandler<AddStockUnitCommand, AddStockUnitCommandResult>
        {
            public async Task<AddStockUnitCommandResult> Handle(AddStockUnitCommand request, CancellationToken cancellationToken)
            {
                await uow.BeginTransactionAsync(cancellationToken);

                try
                {
                    var stockItemId = await stockLookup.FindBySkuAndVariant(request.Sku, request.VariantId, cancellationToken);
                    var stockItem = await repo.GetAsync(stockItemId, cancellationToken);

                    if (stockItem is not null)
                    {
                        var res = stockItem.TryAddUnit(Guid.NewGuid(), request.SerialNumber, request.WarehouseId);

                        if (res.IsSuccess)
                        {
                            await repo.SaveAsync(stockItem, cancellationToken);

                            await outboxRepo.AddRangeAsync(stockItem.DomainEvents, cancellationToken);
                            stockItem.ClearDomainEvents();

                            return new()
                            {
                                Failed = false
                            };
                        }
                    }

                    await uow.RollbackTransactionAsync(cancellationToken);
                    return new()
                    {
                        Failed = true
                    };
                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(cancellationToken);
                    return new()
                    {
                        Failed = true
                    };
                }
                finally
                {
                    await uow.CommitTransactionAsync(cancellationToken);
                }
            }
        }
    }
}
