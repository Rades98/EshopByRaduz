using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Stock.Infrastructure.Common
{
    internal class StockDbFactory : IDesignTimeDbContextFactory<StockDbContext>
    {
        public StockDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockDbContext>();
            optionsBuilder
                .UseSqlServer(string.Empty, b =>
                {
                    b.MigrationsAssembly(Assembly.GetAssembly(typeof(StockDbFactory))!.GetName().Name);
                    b.CommandTimeout(120);
                })
                .EnableSensitiveDataLogging(true);

            return new StockDbContext(optionsBuilder.Options);
        }
    }
}
