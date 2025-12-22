using DomainObjects;

namespace DomainContracts.Events
{
    public sealed record StockItemAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
