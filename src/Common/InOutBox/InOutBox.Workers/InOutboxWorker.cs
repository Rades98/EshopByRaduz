using InOutbox.Orchestrator.Orchestrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InOutBox.Workers
{
    internal sealed class InOutboxWorker<TOrchestrator>(IServiceScopeFactory scopeFactory, ILogger<InOutboxWorker<TOrchestrator>> logger) : BackgroundService
        where TOrchestrator : IOrchestrator
    {
        private readonly TimeSpan _pollInterval = TimeSpan.FromSeconds(10);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var outboxOrchestrator = scope.ServiceProvider.GetRequiredService<TOrchestrator>();

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
