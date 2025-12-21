using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits.Events
{
    public record StockUnitAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
