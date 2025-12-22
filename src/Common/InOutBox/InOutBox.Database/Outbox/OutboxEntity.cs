using InOutBox.Database.Entities;

namespace InOutBox.Database.Outbox
{
    public sealed class OutboxEntity : InOutboxEntity<OutboxEntity>, IInOutboxEntity
    {
        public OutboxEntity()
        {

        }
    }
}
