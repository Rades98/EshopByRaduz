using DomainObjects;

namespace DomainContracts.Events.Basket
{
    public sealed record BasketChangedEvent(Guid UserReference) : DomainEvent;
}
