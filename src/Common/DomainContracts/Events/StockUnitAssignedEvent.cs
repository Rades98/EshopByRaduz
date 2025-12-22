using DomainObjects;

namespace DomainContracts.Events
{
    public sealed record StockUnitAssignedEvent(string Sku, string Variant, Guid OrderReference, string Source) : DomainEvent;
}
