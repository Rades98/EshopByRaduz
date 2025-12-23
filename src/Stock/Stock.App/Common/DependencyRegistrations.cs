using InOutbox.Orchestrator.Orchestrator;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Stock.App.Common.Outbox;
using System.Reflection;

namespace Stock.App.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services, bool addInOutBoxOrchestrators = false)
        {
            services
                .AddMediator(Assembly.GetExecutingAssembly());

            if (addInOutBoxOrchestrators)
            {
                services.AddScoped<IOutboxOrchestrator, OutboxOrchestrator>();
            }

            return services;
        }
    }
}
