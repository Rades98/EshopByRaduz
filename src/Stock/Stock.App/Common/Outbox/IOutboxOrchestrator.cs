namespace Stock.App.Common.Outbox
{
    public interface IOutboxOrchestrator
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
