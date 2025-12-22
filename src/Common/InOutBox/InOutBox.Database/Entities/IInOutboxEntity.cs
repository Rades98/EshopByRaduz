using InOutbox.Orchestrator;

namespace InOutBox.Database.Entities
{
    public interface IInOutboxEntity
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Payload { get; set; }

        public DateTime OccurredAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public InOutboxStatus Status { get; set; }

        public int RetryCount { get; set; }
    }
}
