using DomainContracts.Events.Catalog;
using InOutbox.Orchestrator.Orchestrator;
using InOutbox.Orchestrator.Repos;
using MediatR;
using System.Text.Json;

namespace Regulatory.App.Common
{
    internal class InboxOrchestrator(IInboxRepo inboxRepo, IMediator mediator) : InboxOrchestratorBase(inboxRepo)
    {
        public override Task<bool> HandleEvent(string type, string payload, CancellationToken cancellationToken)
        {
            var succes = false;

            switch (type)
            {
            case nameof(CatalogGroupAddedEvent):
                var @event = JsonSerializer.Deserialize<CatalogGroupAddedEvent>(payload)!;

                break;

            default:
                throw new InvalidOperationException($"Unknown event type: {type}");
            }

            return Task.FromResult(succes);
        }
    }
}
