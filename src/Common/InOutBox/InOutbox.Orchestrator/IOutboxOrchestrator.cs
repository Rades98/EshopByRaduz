namespace InOutbox.Orchestrator
{
    public interface IOutboxOrchestrator
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
