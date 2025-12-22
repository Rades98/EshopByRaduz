using InOutbox.Orchestrator.Repos;

namespace InOutbox.Orchestrator.Orchestrator
{
    public abstract class InboxOrchestratorBase(IInboxRepo inboxRepo) : InOutboxOrchestratorBase(inboxRepo), IInboxOrchestrator
    {
        public abstract Task<bool> HandleEvent(string type, string payload, CancellationToken cancellationToken);

        override protected async Task<bool> Handle(string type, string payload, CancellationToken cancellationToken)
        {
            return await HandleEvent(type, payload, cancellationToken);
        }
    }
}
