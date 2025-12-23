using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Regulatory.Infrastructure.VatCountry
{
    internal class VatRuleEntityConfiguration : IEntityTypeConfiguration<VatRuleEntity>
    {
        public void Configure(EntityTypeBuilder<VatRuleEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(c => c.VatRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.ValidFrom)
                .IsRequired();
        }
    }
}
