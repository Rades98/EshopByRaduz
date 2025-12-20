using Stock.Domain.StockItems.StockUnits;
using System.Text.Json;

namespace Stock.App.Common.Outbox
{
    internal class OutboxOrchestrator(IOutboxRepo outboxRepo, IEventPublisher eventPublisher) : IOutboxOrchestrator
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

        private async Task<bool> SendEvent(string type, string payload, CancellationToken cancellationToken)
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
            }

            return success;
        }
    }
}
