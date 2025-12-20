using Stock.Domain.Common.Outbox;

namespace Stock.App.Common.Outbox
{
    public class OutboxEvent
    {
        public Guid Id { get; private set; }

        public string Type { get; private set; }

        public string Payload { get; private set; }

        public OutboxStatus Status { get; private set; }

        public int RetryCount { get; private set; }

        public DateTime? LockedAt { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private OutboxEvent(string type, string payload, OutboxStatus status)
        {
            Type = type;
            Payload = payload;
            Status = status;
        }

        public static OutboxEvent Rehydrate(
            Guid id,
            string type,
            string payload,
            OutboxStatus status,
            int retryCount,
            DateTime? lockedAt,
            DateTime createdAt)
        {
            var evt = new OutboxEvent(type, payload, status)
            {
                Id = id,
                RetryCount = retryCount,
                LockedAt = lockedAt,
                CreatedAt = createdAt
            };

            return evt;
        }
    }

}
