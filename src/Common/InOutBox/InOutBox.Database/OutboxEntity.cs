namespace InOutBox.Database
{
    public sealed class OutboxEntity : InOutboxEntity<OutboxEntity>, IInOutboxEntity
    {
        public OutboxEntity()
        {

        }
    }
}
