using InOutbox.Orchestrator;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.App.Common.Outbox;
using System.Reflection;

namespace Stock.App.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediator(Assembly.GetExecutingAssembly())
                .AddScoped<IOutboxOrchestrator, OutboxOrchestrator>();

            return services;
        }
    }
}
