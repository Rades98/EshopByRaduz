using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits.Events
{
    public sealed record StockUnitAssignedEvent(string Sku, string Variant, Guid OrderReference, string Source) : DomainEvent;
}
