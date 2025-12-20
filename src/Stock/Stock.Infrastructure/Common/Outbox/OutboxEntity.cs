using DomainObjects;
using Stock.Domain.Common.Outbox;
using System.Text.Json;

namespace Stock.Infrastructure.Common.Outbox
{
    public sealed class OutboxEntity
    {
        public Guid Id { get; set; }

        public required string Type { get; set; }

        public required string Payload { get; set; }

        public DateTime OccurredAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public OutboxStatus Status { get; set; }

        public int RetryCount { get; set; }

        public static OutboxEntity From(IDomainEvent domainEvent)
            => new()
            {
                Id = domainEvent!.EventId,
                Type = domainEvent.GetType().Name,
                Payload = JsonSerializer.Serialize(
                    domainEvent,
                    domainEvent.GetType()),
                OccurredAt = domainEvent.OccurredAt,
                CreatedAt = DateTime.UtcNow,
                ProcessedAt = null,
                Status = OutboxStatus.Pending,
                RetryCount = 0
            };
    }
}
