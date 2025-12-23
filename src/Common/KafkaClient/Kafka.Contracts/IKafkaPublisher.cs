namespace Kafka
{
    public interface IKafkaPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class;
    }
}
