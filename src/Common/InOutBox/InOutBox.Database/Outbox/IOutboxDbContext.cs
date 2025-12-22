using Microsoft.EntityFrameworkCore;

namespace InOutBox.Database.Outbox
{
    public interface IOutboxDbContext
    {
        public DbSet<OutboxEntity> OutboxEvents { get; set; }
    }
}
