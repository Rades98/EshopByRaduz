using DomainObjects;
using Microsoft.EntityFrameworkCore;
using Stock.App.Common.Outbox;
using Stock.Domain.Common.Outbox;
using System.Collections.ObjectModel;

namespace Stock.Infrastructure.Common.Outbox
{
    internal class OutboxRepo(StockDbContext context) : IOutboxRepo
    {
        public Task AddRangeAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken)
            => context.RunWithinTransaction(async dbContext =>
            {
                context.Outbox.AddRange(events.Select(OutboxEntity.From));

                return Task.CompletedTask;

            }, cancellationToken);

        public Task<ReadOnlyCollection<OutboxEvent>> ClaimPendingAndFailedEventsAsync(int batchSize, CancellationToken cancellationToken)
            => context.RunWithinTransaction(async dbContext =>
            {
                var pendingEvents = dbContext.Outbox
                    .Where(e =>
                        (e.Status == OutboxStatus.Pending || (e.Status == OutboxStatus.Failed && e.RetryCount < 5)) &&
                        e.LockedAt == null)
                    .OrderBy(e => e.OccurredAt)
                    .Take(batchSize)
                    .ToList();

                foreach (var e in pendingEvents)
                {
                    e.LockedAt = DateTime.UtcNow;
                    e.Status = OutboxStatus.Processing;
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                return pendingEvents
                    .Select(@event => OutboxEvent.Rehydrate(@event.Id, @event.Type, @event.Payload, @event.Status, @event.RetryCount, @event.LockedAt, @event.CreatedAt))
                    .ToList()
                    .AsReadOnly<OutboxEvent>();

            }, cancellationToken);

        public async Task MarkAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = await context.Outbox.SingleAsync(e => e.Id == eventId, cancellationToken);

            @event.Status = OutboxStatus.Done;
            @event.ProcessedAt = DateTime.UtcNow;


            context.Outbox.Update(@event);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = await context.Outbox.SingleAsync(e => e.Id == eventId, cancellationToken);

            // TODO in case of broken data we might want to archive or delete the event instead of marking it as failed

            @event.Status = OutboxStatus.Failed;
            @event.RetryCount++;

            context.Outbox.Update(@event);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
