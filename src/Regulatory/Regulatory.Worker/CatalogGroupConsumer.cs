using Confluent.Kafka;
using DomainContracts.Events.Catalog;
using InOutbox.Orchestrator.Repos;
using Kafka;

namespace Regulatory.Worker;

internal class CatalogGroupConsumer(ILogger<CatalogGroupConsumer> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await KafkaTopicInitializer.EnsureTopics(configuration.GetConnectionString("kafka")!, "CatalogGroupAddedEvent");

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration.GetConnectionString("kafka"),
            GroupId = "regulatory-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("CatalogGroupAddedEvent");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            }

            var consumeResult = consumer.Consume(stoppingToken);

            try
            {
                var @event = System.Text.Json.JsonSerializer.Deserialize<CatalogGroupAddedEvent>(consumeResult.Message.Value)!;

                using var scope = scopeFactory.CreateScope();
                var inbox = scope.ServiceProvider.GetRequiredService<IInboxRepo>();

                await inbox.AddRangeAsync([@event], stoppingToken);

                consumer.Commit(consumeResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message: {Message}", consumeResult.Message.Value);
            }
        }
    }
}
