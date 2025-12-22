using Microsoft.EntityFrameworkCore;

namespace InOutBox.Database.Inbox
{
    public interface IInboxDbContext
    {
        public DbSet<InboxEntity> InboxEvents { get; set; }
    }
}
