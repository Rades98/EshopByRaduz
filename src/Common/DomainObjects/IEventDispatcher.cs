namespace DomainObjects
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct);
    }
}
