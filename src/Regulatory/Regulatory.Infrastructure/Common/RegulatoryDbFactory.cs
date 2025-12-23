using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Regulatory.Infrastructure.Common
{
    public sealed class RegulatoryDbFactory : IDesignTimeDbContextFactory<RegulatoryDbContext>
    {
        public RegulatoryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RegulatoryDbContext>();
            optionsBuilder
                .UseSqlServer(string.Empty, b =>
                {
                    b.MigrationsAssembly(Assembly.GetAssembly(typeof(RegulatoryDbContext))!.GetName().Name);
                    b.CommandTimeout(120);
                })
                .EnableSensitiveDataLogging(true);

            return new RegulatoryDbContext(optionsBuilder.Options);
        }
    }
}
