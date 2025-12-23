using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pricing.Infrastructure.Pricing.PriceGroup
{
    internal class PriceGroupEntityConfiguration : IEntityTypeConfiguration<PriceGroupEntity>
    {
        public void Configure(EntityTypeBuilder<PriceGroupEntity> builder)
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
