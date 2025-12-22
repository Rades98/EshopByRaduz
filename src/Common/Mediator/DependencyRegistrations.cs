using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
        {
            services
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(assembly);
                });

            return services;
        }
    }
}
