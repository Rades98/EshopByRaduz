using Stock.Domain.StockItems;

namespace Stock.App.StockItems
{
    public interface IStockItemRepo
    {
        Task<StockItemAggregate?> GetAsync(Guid stockItemId, CancellationToken cancellationToken);

        Task AddAsync(StockItemAggregate aggregate, CancellationToken cancellationToken);

        Task SaveAsync(StockItemAggregate aggregate, CancellationToken cancellationToken);
    }
}
