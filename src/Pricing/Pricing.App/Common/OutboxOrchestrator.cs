using DomainContracts.Events.Pricing;
using InOutbox.Orchestrator.Orchestrator;
using InOutbox.Orchestrator.Repos;
using Kafka;
using System.Text.Json;

namespace Pricing.App.Common
{
    internal class OutboxOrchestrator(IOutboxRepo outboxRepo, IKafkaPublisher eventPublisher) : OutboxOrchestratorBase(outboxRepo)
    {
        public override async Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken)
        {
            var success = false;

            switch (type)
            {
            case nameof(PriceItemAddedEvent):
                success = await eventPublisher.PublishAsync(JsonSerializer.Deserialize<PriceItemAddedEvent>(payload)!, cancellationToken);
                break;

            default:
                throw new InvalidOperationException($"Unknown event type: {type}");
            }

            return success;
        }
    }
}
