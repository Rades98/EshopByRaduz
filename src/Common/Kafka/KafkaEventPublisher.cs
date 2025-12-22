using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Kafka
{
    internal class KafkaEventPublisher : IKafkaPublisher, IDisposable
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

        public async Task<bool> PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : class
        {
            try
            {
                ArgumentNullException.ThrowIfNull(@event, nameof(@event));

                var topicName = typeof(TEvent).Name;
                var message = JsonSerializer.Serialize(@event);

                await _producer.ProduceAsync(topicName, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                }, cancellationToken);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
