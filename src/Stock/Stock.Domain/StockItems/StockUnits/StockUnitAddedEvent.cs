using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits
{
    public record StockUnitAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
