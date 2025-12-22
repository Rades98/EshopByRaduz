using Mediator.Request.Query;
using Stock.Domain.StockItems;
using System.Collections.ObjectModel;

namespace Stock.App.StockItems.GetStockCount
{
    public sealed record GetStockCountQuery(ReadOnlyCollection<StockItemCountRequest> Request) : IQuery<ReadOnlyCollection<StockItemCountResponse>>
    {
        internal sealed class GetStockCountQueryHandler(IStockItemLookup stockLookup, IStockItemRepo repo)
            : IQueryHandler<GetStockCountQuery, ReadOnlyCollection<StockItemCountResponse>>
        {
            public async Task<ReadOnlyCollection<StockItemCountResponse>> Handle(GetStockCountQuery request, CancellationToken cancellationToken)
            {
                var results = new List<StockItemCountResponse>();

                foreach (var item in request.Request)
                {

                    var stockItemId = await stockLookup.FindBySkuAndVariant(item.Sku, item.VariantId, cancellationToken);
                    var stockItem = await repo.GetAsync(stockItemId, cancellationToken);

                    results.Add(new StockItemCountResponse(item.Sku, item.VariantId, stockItem is StockItemAggregate agg ? agg.GetAvailableCount() : 0));
                }

                return results.AsReadOnly();
            }
        }
    }
}
