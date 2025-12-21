using Microsoft.EntityFrameworkCore;
using Pricing.Infrastructure.Currency;
using Pricing.Infrastructure.Pricing.PriceItem;

namespace Pricing.Infrastructure.Common
{
    /*
     dotnet ef migrations add Init --project .\src\Pricing\Pricing.Infrastructure\Pricing.Infrastructure.csproj --startup-project .\src\Pricing\Pricing.Infrastructure\Pricing.Infrastructure.csproj
     */

    public sealed class PricingDbContext(DbContextOptions<PricingDbContext> options) : DbContext(options)
    {
        public DbSet<CurrencyEntity> Currencies { get; set; } = null!;
        public DbSet<PriceItemEntity> PriceItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            new CurrencyEntityConfiguration().Configure(modelBuilder.Entity<CurrencyEntity>());
            new PriceItemEntityConfiguration().Configure(modelBuilder.Entity<PriceItemEntity>());
        }
    }
}
