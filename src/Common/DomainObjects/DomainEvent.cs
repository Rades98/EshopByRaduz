namespace DomainObjects
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public Guid EventId { get; } = Guid.NewGuid();
    }
}
