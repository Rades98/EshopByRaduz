namespace DomainObjects
{
    public interface IDomainEvent
    {
        DateTime OccurredAt { get; }
    }
}
