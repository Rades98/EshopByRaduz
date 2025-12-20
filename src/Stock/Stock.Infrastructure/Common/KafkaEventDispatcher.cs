using DomainObjects;
using Stock.App.Common;

namespace Stock.Infrastructure.Common
{
    internal class KafkaEventDispatcher(IEventPublisher eventPub) : IEventDispatcher
    {
        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct)
        {
            foreach (var @event in events)
            {
                await eventPub.PublishAsync(@event, ct);
            }
        }
    }
}
