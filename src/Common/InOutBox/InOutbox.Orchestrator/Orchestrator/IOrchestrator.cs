namespace InOutbox.Orchestrator.Orchestrator
{
    public interface IOrchestrator
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
