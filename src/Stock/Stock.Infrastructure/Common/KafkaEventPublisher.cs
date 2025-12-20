using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Stock.App.Common;
using System.Text.Json;

namespace Stock.Infrastructure.Common
{
    internal class KafkaEventPublisher : IEventPublisher, IDisposable
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration.GetConnectionString("kafka")
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : class
        {

            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var topicName = typeof(TEvent).Name;
            var message = JsonSerializer.Serialize(@event);

            await _producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            }, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
