namespace InOutbox.Orchestrator
{
    public class InOutboxEvent
    {
        public Guid Id { get; private set; }

        public string Type { get; private set; }

        public string Payload { get; private set; }

        public InOutboxStatus Status { get; private set; }

        public int RetryCount { get; private set; }

        public DateTime? LockedAt { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private InOutboxEvent(string type, string payload, InOutboxStatus status)
        {
            Type = type;
            Payload = payload;
            Status = status;
        }

        public static InOutboxEvent Rehydrate(
            Guid id,
            string type,
            string payload,
            InOutboxStatus status,
            int retryCount,
            DateTime? lockedAt,
            DateTime createdAt)
        {
            var evt = new InOutboxEvent(type, payload, status)
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
