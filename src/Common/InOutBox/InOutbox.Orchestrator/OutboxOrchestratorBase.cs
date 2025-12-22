namespace InOutbox.Orchestrator
{
    public abstract class OutboxOrchestratorBase(IOutboxRepo outboxRepo) : IOutboxOrchestrator
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var events = await outboxRepo.ClaimPendingAndFailedEventsAsync(50, cancellationToken);

            foreach (var e in events)
            {
                if (await SendEvent(e.Type, e.Payload, cancellationToken))
                {
                    await outboxRepo.MarkAsPublishedAsync(e.Id, cancellationToken);
                }
                else
                {
                    await outboxRepo.MarkAsFailedAsync(e.Id, cancellationToken);
                }
            }
        }

        public abstract Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken);
    }
}
