using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits
{
    public record StockUnitAddedEvent(Guid Id, Guid StockUnitId, Guid WarehouseId) : DomainEvent;
}
