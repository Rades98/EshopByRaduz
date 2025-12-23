namespace DomainObjects
{
    public record DomainEventVithValidity(DateTime ValidFrom, DateTime? ValidTo) : DomainEvent;
}
