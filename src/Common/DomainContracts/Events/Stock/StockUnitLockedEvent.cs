using DomainObjects;

namespace DomainContracts.Events.Stock
{
    public record StockUnitLockedEvent(string Sku, string Variant, string Source) : DomainEvent;
}
