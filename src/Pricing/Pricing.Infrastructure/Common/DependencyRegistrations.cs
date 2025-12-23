using Database.SQL;
using InOutBox.Database.Extensions;
using Kafka;
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
            services.AddSqlDatabase<PricingDbContext>(configuration, env, "PricingDatabase");

            services.AddInboxRepo<PricingDbContext>();
            services.AddOutboxRepo<PricingDbContext>();

            services.AddUow<PricingDbContext>();

            services.AddTransient<IPricingRepo, PricingRepo>();
            services.AddTransient<IPricingLookup, PricingLookup>();

            services.AddKafkaPublisher();

            return services;
        }
    }
}
