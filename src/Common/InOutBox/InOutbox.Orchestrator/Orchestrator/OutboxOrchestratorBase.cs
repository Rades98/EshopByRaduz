using InOutbox.Orchestrator.Repos;

namespace InOutbox.Orchestrator.Orchestrator
{
    public abstract class OutboxOrchestratorBase(IOutboxRepo outboxRepo) : InOutboxOrchestratorBase(outboxRepo), IOutboxOrchestrator
    {
        public abstract Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken);

        override protected async Task<bool> Handle(string type, string payload, CancellationToken cancellationToken)
        {
            return await SendEvent(type, payload, cancellationToken);
        }
    }
}
