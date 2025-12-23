using DomainObjects;

namespace DomainContracts.Events.Stock
{
    public sealed record StockUnitAssignedEvent(string Sku, string Variant, Guid OrderReference, string Source) : DomainEvent;
}
