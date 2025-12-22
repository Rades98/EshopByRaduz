using Confluent.Kafka;
using DomainContracts.Events;
using Kafka;
using MediatR;
using Pricing.App.Pricing.AddPriceForProduct;

namespace Pricing.Consumer;

internal class Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await KafkaTopicInitializer.EnsureTopics(configuration.GetConnectionString("kafka")!, "StockItemAddedEvent");

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration.GetConnectionString("kafka"),
            GroupId = "pricing-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
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

            var @event = System.Text.Json.JsonSerializer.Deserialize<StockItemAddedEvent>(consumeResult.Message.Value)!;

            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var res = await mediator.Send<bool>(new AddPriceForProductCommand(@event.Sku, @event.Variant), stoppingToken);

            if (res)
            {
                // OK
            }
            else
            {
                // Handle failure - retry etc there should be Inbox btw
            }
        }

        consumer.Close();
    }
}
