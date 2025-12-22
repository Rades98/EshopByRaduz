using Database.SQL;
using DomainObjects;
using InOutbox.Orchestrator;
using InOutbox.Orchestrator.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;

namespace InOutBox.Database.Outbox
{
    public class OutboxRepo<TDbContext>(TDbContext context) : IOutboxRepo
        where TDbContext : DbContext, IOutboxDbContext
    {
        public Task AddRangeAsync(IReadOnlyCollection<IDomainEvent> events, CancellationToken cancellationToken)
            => context.AddManyWithinTransaction<OutboxEntity, TDbContext>(async dbContext =>
            {
                var addedEntries = new List<EntityEntry<OutboxEntity>>();

                foreach (var item in events.Select(OutboxEntity.From))
                {
                    addedEntries.Add(await dbContext.AddAsync(item, cancellationToken));
                }

                return addedEntries;

            }, cancellationToken);

        public async Task<ReadOnlyCollection<InOutboxEvent>> ClaimPendingAndFailedEventsAsync(int batchSize, CancellationToken cancellationToken)
            => await context.RunWithinTransaction(async dbContext =>
            {
                var pendingEvents = dbContext.OutboxEvents
                    .Where(e =>
                        (e.Status == InOutboxStatus.Pending || e.Status == InOutboxStatus.Failed && e.RetryCount < 5) &&
                        e.LockedAt == null || e.Status == InOutboxStatus.Processing && e.LockedAt > DateTime.UtcNow.AddMinutes(2))
                    .OrderBy(e => e.OccurredAt)
                    .Take(batchSize)
                    .ToList();

                foreach (var e in pendingEvents)
                {
                    e.LockedAt = DateTime.UtcNow;
                    e.Status = InOutboxStatus.Processing;
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                return pendingEvents
                    .Select(@event => InOutboxEvent.Rehydrate(@event.Id, @event.Type, @event.Payload, @event.Status, @event.RetryCount, @event.LockedAt, @event.CreatedAt))
                    .ToList()
                    .AsReadOnly<InOutboxEvent>();

            }, cancellationToken) ?? new ReadOnlyCollection<InOutboxEvent>([]);

        public async Task MarkAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = await context.OutboxEvents.SingleAsync(e => e.Id == eventId, cancellationToken);

            @event.Status = InOutboxStatus.Done;
            @event.ProcessedAt = DateTime.UtcNow;


            context.OutboxEvents.Update(@event);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = await context.OutboxEvents.SingleAsync(e => e.Id == eventId, cancellationToken);

            // TODO in case of broken data we might want to archive or delete the event instead of marking it as failed

            @event.Status = InOutboxStatus.Failed;
            @event.RetryCount++;

            context.OutboxEvents.Update(@event);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
