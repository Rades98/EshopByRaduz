using DomainObjects;

namespace DomainContracts.Events
{
    public record StockUnitLockedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
