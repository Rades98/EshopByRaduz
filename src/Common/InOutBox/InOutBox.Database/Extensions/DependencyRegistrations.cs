using InOutbox.Orchestrator.Repos;
using InOutBox.Database.Inbox;
using InOutBox.Database.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InOutBox.Database.Extensions
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddInboxRepo<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IInboxDbContext
        {
            services.AddScoped<IInboxRepo, InboxRepo<TDbContext>>();

            return services;
        }

        public static IServiceCollection AddOutboxRepo<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IOutboxDbContext
        {
            services.AddScoped<IOutboxRepo, OutboxRepo<TDbContext>>();

            return services;
        }
    }
}
