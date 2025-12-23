using Confluent.Kafka;
using DomainContracts.Events.Stock;
using InOutbox.Orchestrator.Repos;
using Kafka;

namespace Pricing.Worker;

internal class StockItemConsumer(ILogger<StockItemConsumer> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await KafkaTopicInitializer.EnsureTopics(configuration.GetConnectionString("kafka")!, "StockItemAddedEvent");

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration.GetConnectionString("kafka"),
            GroupId = "pricing-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("StockItemAddedEvent");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            }

            var consumeResult = consumer.Consume(stoppingToken);

            try
            {
                var @event = System.Text.Json.JsonSerializer.Deserialize<StockItemAddedEvent>(consumeResult.Message.Value)!;

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

        consumer.Close();
    }
}
