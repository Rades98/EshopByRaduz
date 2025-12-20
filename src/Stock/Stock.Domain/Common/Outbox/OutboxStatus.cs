namespace Stock.Domain.Common.Outbox
{
    public enum OutboxStatus
    {
        Pending = 0,
        Processing = 1,
        Done = 2,
        Failed = 3
    }
}
