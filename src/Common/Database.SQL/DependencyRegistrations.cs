using Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Database.SQL
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddUow<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.TryAddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();

            return services;
        }


        public static IServiceCollection AddSqlDatabase<TDbContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, string connString)
                where TDbContext : DbContext
        {
            services.AddDbContextPool<TDbContext>(
                (serviceProvider, options) =>
                {
                    options.UseSqlServer(configuration.GetConnectionString(connString), builder =>
                    {
                        builder.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                        builder.CommandTimeout(120);
                    })
                    .EnableSensitiveDataLogging(!env.IsProduction());
                });

            return services;
        }
    }
}
