using Mediator.Request.Query;
using Stock.Domain.StockItems;

namespace Stock.App.StockItems.StockUnits
{
    public sealed record GetStockCountQuery(string Sku, string VariantId) : IQuery<int>
    {
        internal sealed class GetStockCountQueryHandler(IStockItemLookup stockLookup, IStockItemRepo repo) : IQueryHandler<GetStockCountQuery, int>
        {
            public async Task<int> Handle(GetStockCountQuery request, CancellationToken cancellationToken)
            {
                var stockItemId = await stockLookup.FindBySkuAndVariant(request.Sku, request.VariantId, cancellationToken).ConfigureAwait(false);
                var stockItem = await repo.GetAsync(stockItemId, cancellationToken).ConfigureAwait(false);

                return stockItem is StockItemAggregate agg ? agg.GetAvailableCount() : 0;
            }
        }
    }
}
