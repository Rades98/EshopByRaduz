using InOutBox.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InOutBox.Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureInOutBoxEntity<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, IInOutboxEntity
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            new InOutboxEntityConfiguration<TEntity>().Configure(modelBuilder.Entity<TEntity>());
        }
    }
}
