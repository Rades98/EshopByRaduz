namespace InOutbox.Orchestrator
{
    public enum InOutboxStatus
    {
        Pending = 0,
        Processing = 1,
        Done = 2,
        Failed = 3
    }
}
