using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InOutBox.Database.Entities
{
    public sealed class InOutboxEntityConfiguration<TInOutBoxEntity> : IEntityTypeConfiguration<TInOutBoxEntity>
        where TInOutBoxEntity : class, IInOutboxEntity
    {
        public void Configure(EntityTypeBuilder<TInOutBoxEntity> builder)
        {
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
