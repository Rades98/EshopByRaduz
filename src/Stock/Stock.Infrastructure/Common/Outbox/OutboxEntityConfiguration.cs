using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure.Common.Outbox
{
    internal class OutboxEntityConfiguration : IEntityTypeConfiguration<OutboxEntity>
    {
        public void Configure(EntityTypeBuilder<OutboxEntity> builder)
        {
            builder.ToTable("OutboxEvents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.RetryCount)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasIndex(x => new { x.Status, x.CreatedAt })
                .HasDatabaseName("IX_Outbox_Status_CreatedAt");
        }
    }
}
