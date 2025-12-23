using DomainObjects;

namespace DomainContracts.Events.Stock
{
    public record StockUnitAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
