using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pricing.Infrastructure.Pricing.PriceItem
{
    internal class PriceItemEntityConfiguration : IEntityTypeConfiguration<PriceItemEntity>
    {
        public void Configure(EntityTypeBuilder<PriceItemEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.VariantId)
                .HasMaxLength(50);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(p => p.ValidFrom)
                .IsRequired();

            builder.Property(p => p.ValidTo);

            builder.Property(p => p.PriceType)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(p => p.Currency)
                .WithMany()
                .HasForeignKey(p => p.CurrencyCode)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => new { p.Sku, p.VariantId, p.PriceType, p.CurrencyCode })
                   .IsUnique();
        }
    }
}
