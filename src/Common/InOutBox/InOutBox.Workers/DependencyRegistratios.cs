using InOutbox.Orchestrator.Orchestrator;
using Microsoft.Extensions.DependencyInjection;

namespace InOutBox.Workers
{
    public static class DependencyRegistratios
    {
        public static IServiceCollection AddOutboxWorker(this IServiceCollection services)
        {
            services.AddHostedService<InOutboxWorker<IOutboxOrchestrator>>();

            return services;
        }

        public static IServiceCollection AddInboxWorker(this IServiceCollection services)
        {
            services.AddHostedService<InOutboxWorker<IInboxOrchestrator>>();

            return services;
        }
    }
}
