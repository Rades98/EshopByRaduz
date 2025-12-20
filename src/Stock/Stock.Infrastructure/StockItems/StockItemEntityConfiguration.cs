using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure.StockItems
{
    internal class StockItemEntityConfiguration : IEntityTypeConfiguration<StockItemEntity>
    {
        public void Configure(EntityTypeBuilder<StockItemEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.VariantId)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
