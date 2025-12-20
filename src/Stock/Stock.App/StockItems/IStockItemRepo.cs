using Stock.Domain.StockItems;

namespace Stock.App.StockItems
{
    public interface IStockItemRepo
    {
        Task<StockItemAggregate?> GetAsync(Guid stockItemId, CancellationToken ct);

        Task AddAsync(StockItemAggregate aggregate, CancellationToken ct);

        Task SaveAsync(StockItemAggregate aggregate, CancellationToken ct);
    }
}
