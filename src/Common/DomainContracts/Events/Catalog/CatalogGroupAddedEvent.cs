using DomainObjects;

namespace DomainContracts.Events.Catalog
{
    public sealed record CatalogGroupAddedEvent(string Name) : DomainEvent;
}
