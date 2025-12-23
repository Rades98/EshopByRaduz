using Kafka;
using Pricing.App.Common;

namespace Pricing.Infrastructure.Common
{
    internal class EventPublisher(IKafkaPublisher publisher) : IEventPublisher
    {
        public Task<bool> PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class
            => publisher.PublishAsync(@event, cancellationToken);
    }
}
