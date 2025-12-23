using InOutbox.Orchestrator.Orchestrator;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Pricing.App.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediator(Assembly.GetExecutingAssembly());
            services.TryAddScoped<IInboxOrchestrator, InboxOrchestrator>();
            services.TryAddScoped<IOutboxOrchestrator, OutboxOrchestrator>();

            return services;
        }
    }
}
