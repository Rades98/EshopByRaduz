using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Regulatory.Infrastructure
{
    internal class RegulatoryEntityConfiguration : IEntityTypeConfiguration<RegulatoryEntity>
    {
        public void Configure(EntityTypeBuilder<RegulatoryEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(t => t.ProductGroup)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.IsActive)
               .HasDefaultValue(false);

            builder.HasMany(p => p.VatRules)
                .WithOne(g => g.Regulatory)
                .HasForeignKey(p => p.RegulatoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
