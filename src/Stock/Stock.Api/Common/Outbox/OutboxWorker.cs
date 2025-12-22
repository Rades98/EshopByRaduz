using InOutbox.Orchestrator;

namespace Stock.Api.Common.Outbox
{
    internal sealed class OutboxWorker(IServiceScopeFactory scopeFactory, ILogger<OutboxWorker> logger) : BackgroundService
    {
        private readonly TimeSpan _pollInterval = TimeSpan.FromSeconds(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var outboxOrchestrator = scope.ServiceProvider.GetRequiredService<IOutboxOrchestrator>();

                    await outboxOrchestrator.ExecuteAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // graceful shutdown
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Outbox worker failed");
                }

                await Task.Delay(_pollInterval, stoppingToken);
            }
        }
    }

}
