using DomainObjects;
using System.Collections.ObjectModel;

namespace Stock.App.Common.Outbox
{
    public interface IOutboxRepo
    {
        Task AddRangeAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken);

        Task<ReadOnlyCollection<OutboxEvent>> ClaimPendingAndFailedEventsAsync(int batchSize, CancellationToken cancellationToken);

        Task MarkAsPublishedAsync(Guid eventId, CancellationToken cancellationToken);

        Task MarkAsFailedAsync(Guid eventId, CancellationToken cancellationToken);
    }
}
