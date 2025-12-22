using InOutBox.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pricing.App.Pricing;
using Pricing.Infrastructure.Pricing;

namespace Pricing.Infrastructure.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterInfraStructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddDbContextPool<PricingDbContext>(
                (serviceProvider, options) =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("PricingDatabase"), builder =>
                    {
                        builder.MigrationsAssembly(typeof(PricingDbContext).Assembly.FullName);
                        builder.CommandTimeout(120);
                    })
                    .EnableSensitiveDataLogging(!env.IsProduction());
                });

            services.AddInboxRepo<PricingDbContext>();

            services.AddTransient<IPricingRepo, PricingRepo>();
            services.AddTransient<IPricingLookup, PricingLookup>();


            return services;
        }
    }
}
