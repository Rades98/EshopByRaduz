using DomainObjects;

namespace DomainContracts.Events.Stock
{
    public sealed record StockItemAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
