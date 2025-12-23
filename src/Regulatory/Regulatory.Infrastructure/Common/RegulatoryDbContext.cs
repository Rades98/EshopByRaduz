using InOutBox.Database.Extensions;
using InOutBox.Database.Inbox;
using InOutBox.Database.Outbox;
using Microsoft.EntityFrameworkCore;
using Regulatory.Infrastructure.VatCountry;

namespace Regulatory.Infrastructure.Common
{
    /*
     dotnet ef migrations add Init --project .\src\Regulatory\Regulatory.Infrastructure\Regulatory.Infrastructure.csproj --startup-project .\src\Regulatory\Regulatory.Infrastructure\Regulatory.Infrastructure.csproj
     */
    public sealed class RegulatoryDbContext(DbContextOptions<RegulatoryDbContext> options) : DbContext(options), IOutboxDbContext, IInboxDbContext
    {
        public DbSet<OutboxEntity> OutboxEvents { get; set; } = null!;

        public DbSet<InboxEntity> InboxEvents { get; set; } = null!;

        public DbSet<RegulatoryEntity> Regulatories { get; set; } = null!;

        public DbSet<VatRuleEntity> VatRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.ConfigureInOutBoxEntity<OutboxEntity>();
            modelBuilder.ConfigureInOutBoxEntity<InboxEntity>();

            modelBuilder.ApplyConfiguration(new VatRuleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RegulatoryEntityConfiguration());
        }
    }
}
