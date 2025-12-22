using DomainObjects;
using InOutbox.Orchestrator;
using System.Text.Json;

namespace InOutBox.Database.Entities
{
    public class InOutboxEntity<TInOutboxEntity>
        where TInOutboxEntity : IInOutboxEntity, new()
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = null!;

        public string Payload { get; set; } = null!;

        public DateTime OccurredAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public InOutboxStatus Status { get; set; }

        public int RetryCount { get; set; }

        public static TInOutboxEntity From(IDomainEvent domainEvent)
            => domainEvent is not null ? new()
            {
                Id = domainEvent!.EventId,
                Type = domainEvent.GetType().Name,
                Payload = JsonSerializer.Serialize(
                    domainEvent,
                    domainEvent.GetType()),
                OccurredAt = domainEvent.OccurredAt,
                CreatedAt = DateTime.UtcNow,
                ProcessedAt = null,
                Status = InOutboxStatus.Pending,
                RetryCount = 0
            } : throw new ArgumentNullException(nameof(domainEvent));
    }
}
