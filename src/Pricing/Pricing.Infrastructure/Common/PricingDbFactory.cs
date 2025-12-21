using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Pricing.Infrastructure.Common
{
    internal class PricingDbFactory : IDesignTimeDbContextFactory<PricingDbContext>
    {
        public PricingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PricingDbContext>();
            optionsBuilder
                .UseSqlServer(string.Empty, b =>
                {
                    b.MigrationsAssembly(Assembly.GetAssembly(typeof(PricingDbContext))!.GetName().Name);
                    b.CommandTimeout(120);
                })
                .EnableSensitiveDataLogging(true);

            return new PricingDbContext(optionsBuilder.Options);
        }
    }
}
