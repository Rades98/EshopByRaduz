using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Kafka
{
    public static class KafkaTopicInitializer
    {
        public static async Task EnsureTopics(string bootstrapServers, string name)
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = bootstrapServers
            };

            using var admin = new AdminClientBuilder(adminConfig).Build();

            var registeredTopics = admin.GetMetadata(TimeSpan.FromSeconds(10)).Topics.Select(x => x.Topic);

            if (!registeredTopics.Contains(name))
            {
                var topic = new TopicSpecification
                {
                    Name = name,
                    NumPartitions = 1,
                    ReplicationFactor = -1,
                };

                try
                {
                    await admin.CreateTopicsAsync([topic]);
                }
                catch (Exception)
                {
                    // mlah
                }
            }

        }
    }
}
