using DomainObjects;
using System.Collections.ObjectModel;

namespace InOutbox.Orchestrator.Repos
{
    public interface IInOutboxRepo
    {
        Task AddRangeAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken);

        Task<ReadOnlyCollection<InOutboxEvent>> ClaimPendingAndFailedEventsAsync(int batchSize, CancellationToken cancellationToken);

        Task MarkAsPublishedAsync(Guid eventId, CancellationToken cancellationToken);

        Task MarkAsFailedAsync(Guid eventId, CancellationToken cancellationToken);
    }
}
