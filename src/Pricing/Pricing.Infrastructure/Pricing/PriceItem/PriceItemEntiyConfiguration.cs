using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pricing.Infrastructure.Pricing.PriceItem
{
    internal sealed class PriceItemEntityConfiguration : IEntityTypeConfiguration<PriceItemEntity>
    {
        public void Configure(EntityTypeBuilder<PriceItemEntity> builder)
        {
            builder.HasKey(p => p.Id);

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
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Group)
                .WithMany(g => g.Items)
                .HasForeignKey(p => p.PriceGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
