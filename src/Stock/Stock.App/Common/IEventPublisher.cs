namespace Stock.App.Common
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class;
    }
}
