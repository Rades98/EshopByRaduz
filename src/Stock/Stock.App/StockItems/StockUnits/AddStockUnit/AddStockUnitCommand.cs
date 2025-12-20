using Mediator.Request.Command;
using Stock.App.Common;

namespace Stock.App.StockItems.StockUnits.AddStockUnit
{
    public sealed record AddStockUnitCommand(string SerialNumber, string Sku, string VariantId, Guid WarehouseId) : ICommand<bool>
    {
        internal sealed class AddStockUnitCommandHandler(IStockItemLookup stockLookup, IStockItemRepo repo, IEventPublisher eventPub) : ICommandHandler<AddStockUnitCommand, bool>
        {
            public async Task<bool> Handle(AddStockUnitCommand request, CancellationToken cancellationToken)
            {
                var stockItemId = await stockLookup.FindBySkuAndVariant(request.Sku, request.VariantId, cancellationToken);
                var stockItem = await repo.GetAsync(stockItemId, cancellationToken);

                if (stockItem is not null)
                {
                    stockItem.TryAddUnit(Guid.NewGuid(), request.SerialNumber, request.WarehouseId);

                    await repo.SaveAsync(stockItem, cancellationToken);
                }


            }
        }
    }
}
