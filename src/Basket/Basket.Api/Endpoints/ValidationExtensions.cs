using Basket.Api.Endpoints.RequestModels;
using Basket.Api.Services;
using Stock.Grpc;

namespace Basket.Api.Endpoints
{
    internal static class ValidationExtensions
    {
        internal sealed record InsufficientStockItem(string Sku, string Variant);

        internal static async Task<List<InsufficientStockItem>> GetInvalidStockItems(this BasketRequestModel req, StockGrpcService stockGrpcService, CancellationToken ct)
        {
            var stockRequest = new StockCountRequest();

            foreach (var item in req.Items.GroupBy(x => new { x.Sku, x.Variant }))
            {
                stockRequest.Items.Add(new StockItem
                {
                    Sku = item.Key.Sku,
                    Variation = item.Key.Variant
                });
            }

            var stockRes = await stockGrpcService.GetStockCountAsync(stockRequest, ct);

            var stockLookup = stockRes.Stocks.ToDictionary(x => (x.Item.Sku, x.Item.Variation), x => x.Available);

            return [.. req.Items
                .GroupBy(x => new { x.Sku, x.Variant })
                .Select(g =>
                {
                    return new
                    {
                        g.Key.Sku,
                        g.Key.Variant,
                        Requested = g.Sum(x => x.Quantity),
                        Available = stockLookup.TryGetValue((g.Key.Sku, g.Key.Variant),out var avail) ? avail: 0
                    };
        })
                .Where(x => x.Requested > x.Available)
                .Select(x => new InsufficientStockItem(x.Sku, x.Variant))];
        }
    }
}
