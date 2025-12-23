using InOutbox.Orchestrator.Repos;
using InOutBox.Database.Inbox;
using InOutBox.Database.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InOutBox.Database.Extensions
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddInboxRepo<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IInboxDbContext
        {
            services.TryAddScoped<IInboxRepo, InboxRepo<TDbContext>>();

            return services;
        }

        public static IServiceCollection AddOutboxRepo<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IOutboxDbContext
        {
            services.TryAddScoped<IOutboxRepo, OutboxRepo<TDbContext>>();

            return services;
        }
    }
}
