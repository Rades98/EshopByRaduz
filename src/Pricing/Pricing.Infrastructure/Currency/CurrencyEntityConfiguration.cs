using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pricing.Infrastructure.Currency
{
    internal class CurrencyEntityConfiguration : IEntityTypeConfiguration<CurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.HasKey(c => c.Code);

            builder.Property(c => c.Code)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(c => c.Symbol)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(c => c.ExchangeRate)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(c => c.Precision)
                .IsRequired();

            builder.Property(c => c.IsMaster)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.HasIndex(c => c.IsMaster);
        }
    }
}
