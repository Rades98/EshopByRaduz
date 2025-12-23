using InOutbox.Orchestrator.Orchestrator;
using InOutbox.Orchestrator.Repos;
using Kafka;

namespace Regulatory.App.Common
{
    internal class OutboxOrchestrator(IOutboxRepo outboxRepo, IKafkaPublisher eventPublisher) : OutboxOrchestratorBase(outboxRepo)
    {
        public override Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken)
        {
            var succes = false;
            return Task.FromResult(succes);
        }
    }
}
