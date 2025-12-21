using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits.Events
{
    public record StockUnitLockedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
