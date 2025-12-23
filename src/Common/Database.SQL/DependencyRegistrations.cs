using Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    }
}
