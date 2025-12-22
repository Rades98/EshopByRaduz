using DomainContracts.Events;
using InOutbox.Orchestrator.Orchestrator;
using InOutbox.Orchestrator.Repos;
using MediatR;
using Pricing.App.Pricing.AddPriceForProduct;
using System.Text.Json;

namespace Pricing.App.Common
{
    internal class InboxOrchestrator(IInboxRepo inboxRepo, IMediator mediator) : InboxOrchestratorBase(inboxRepo)
    {
        public override async Task<bool> HandleEvent(string type, string payload, CancellationToken cancellationToken)
        {
            var success = false;

            switch (type)
            {
            case nameof(StockItemAddedEvent):
                var @event = JsonSerializer.Deserialize<StockUnitAddedEvent>(payload)!;
                success = await mediator.Send<bool>(new AddPriceForProductCommand(@event.Sku, @event.Variant), cancellationToken); ;
                break;

            default:
                throw new InvalidOperationException($"Unknown event type: {type}");
            }

            return success;
        }
    }
}
