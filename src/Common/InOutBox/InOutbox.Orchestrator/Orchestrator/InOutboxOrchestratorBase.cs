using InOutbox.Orchestrator.Repos;

namespace InOutbox.Orchestrator.Orchestrator
{
    public abstract class InOutboxOrchestratorBase(IInOutboxRepo outboxRepo)
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var events = await outboxRepo.ClaimPendingAndFailedEventsAsync(50, cancellationToken);

            foreach (var e in events)
            {
                if (await Handle(e.Type, e.Payload, cancellationToken))
                {
                    await outboxRepo.MarkAsPublishedAsync(e.Id, cancellationToken);
                }
                else
                {
                    await outboxRepo.MarkAsFailedAsync(e.Id, cancellationToken);
                }
            }
        }

        protected abstract Task<bool> Handle(string type, string payload, CancellationToken cancellationToken);
    }
}
