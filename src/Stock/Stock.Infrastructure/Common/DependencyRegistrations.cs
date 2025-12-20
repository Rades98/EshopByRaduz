using Mediator.Request.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stock.App.Common;
using Stock.App.Common.Outbox;
using Stock.App.StockItems;
using Stock.Infrastructure.Common.Outbox;
using Stock.Infrastructure.StockItems;

namespace Stock.Infrastructure.Common
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterInfraStructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddDbContextPool<StockDbContext>(
                (serviceProvider, options) =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("StockDatabase"), builder =>
                    {
                        builder.MigrationsAssembly(typeof(StockDbFactory).Assembly.FullName);
                        builder.CommandTimeout(120);
                    })
                    .EnableSensitiveDataLogging(!env.IsProduction());
                });

            services.AddTransient<IStockItemRepo, StockItemRepo>();
            services.AddTransient<IStockItemLookup, StockItemLookup>();

            services.AddScoped<ITransactionUnitOfWork, UnitOfWork>();
            services.AddScoped<IOutboxRepo, OutboxRepo>();

            services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

            return services;
        }
    }
}
