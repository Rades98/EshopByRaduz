using DomainContracts.Events.Stock;
using InOutbox.Orchestrator.Orchestrator;
using InOutbox.Orchestrator.Repos;
using Kafka;
using System.Text.Json;

namespace Stock.App.Common.Outbox
{
    internal class OutboxOrchestrator(IOutboxRepo outboxRepo, IKafkaPublisher eventPublisher) : OutboxOrchestratorBase(outboxRepo)
    {
        public override async Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken)
        {
            var success = false;

            switch (type)
            {
            case nameof(StockUnitAddedEvent):
                success = await eventPublisher.PublishAsync(JsonSerializer.Deserialize<StockUnitAddedEvent>(payload)!, cancellationToken);
                break;

            case nameof(StockUnitLockedEvent):
                success = await eventPublisher.PublishAsync(JsonSerializer.Deserialize<StockUnitLockedEvent>(payload)!, cancellationToken);
                break;

            case nameof(StockUnitAssignedEvent):
                success = await eventPublisher.PublishAsync(JsonSerializer.Deserialize<StockUnitAssignedEvent>(payload)!, cancellationToken);
                break;

            case nameof(StockItemAddedEvent):
                success = await eventPublisher.PublishAsync(JsonSerializer.Deserialize<StockItemAddedEvent>(payload)!, cancellationToken);
                break;

            default:
                throw new InvalidOperationException($"Unknown event type: {type}");
            }

            return success;
        }
    }
}
