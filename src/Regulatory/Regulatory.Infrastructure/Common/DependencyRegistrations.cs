using Database.SQL;
using InOutBox.Database.Extensions;
using Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Regulatory.Infrastructure.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterInfraStructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddSqlDatabase<RegulatoryDbContext>(configuration, env, "RegulatoryDatabase");

            services.AddOutboxRepo<RegulatoryDbContext>();
            services.AddInboxRepo<RegulatoryDbContext>();

            services.AddUow<RegulatoryDbContext>();

            services.AddKafkaPublisher();

            return services;
        }
    }
}
