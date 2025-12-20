using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.StockItems.StockUnits;

namespace Stock.Infrastructure.StockItems.StockUnits
{
    internal class StockUnitEntityConfiguration : IEntityTypeConfiguration<StockUnitEntity>
    {
        public void Configure(EntityTypeBuilder<StockUnitEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SerialNumber)
                .IsRequired()
                .HasMaxLength(SerialNumberValueObject.MaxLength);

            builder.Property(x => x.IsLocked)
                .IsRequired();

            builder.HasOne(x => x.StockItem)
                .WithMany(x => x.Units)
                .HasForeignKey(x => x.StockItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasIndex(x => new { x.StockItemId, x.SerialNumber })
                .HasDatabaseName("IX_StockUnit_StockItemId_SerialNumber")
                .IsUnique();
        }
    }
}
