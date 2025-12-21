using DomainObjects;

namespace Stock.Domain.StockItems.Events
{
    public sealed record StockItemAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
