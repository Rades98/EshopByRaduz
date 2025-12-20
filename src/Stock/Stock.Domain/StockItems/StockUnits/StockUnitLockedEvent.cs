using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits
{
    public record StockUnitLockedEvent(Guid Id) : DomainEvent;
}
