using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.StockItems;
using Stock.Infrastructure.StockItems.StockUnits;
using Stock.Infrastructure.Warehouses;

namespace Stock.Infrastructure.Common
{
    /*
     dotnet ef migrations add Init --project .\src\Stock\Stock.Infrastructure\Stock.Infrastructure.csproj --startup-project .\src\Stock\Stock.Infrastructure\Stock.Infrastructure.csproj
     */

    public sealed class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
    {
        public DbSet<WarehouseEntity> Warehouses { get; set; } = null!;

        public DbSet<StockItemEntity> StockItems { get; set; } = null!;

        public DbSet<StockUnitEntity> StockUnits { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            new WarehouseEntityConfiguration().Configure(modelBuilder.Entity<WarehouseEntity>());
            new StockItemEntityConfiguration().Configure(modelBuilder.Entity<StockItemEntity>());
            new StockUnitEntityConfiguration().Configure(modelBuilder.Entity<StockUnitEntity>());
        }
    }
}
