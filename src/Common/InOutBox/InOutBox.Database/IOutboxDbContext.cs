using Microsoft.EntityFrameworkCore;

namespace InOutBox.Database
{
    public interface IOutboxDbContext
    {
        public DbSet<OutboxEntity> OutboxEvents { get; set; }
    }
}
