using DomainObjects;

namespace DomainContracts.Events
{
    public record StockUnitAddedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
