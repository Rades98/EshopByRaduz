using Microsoft.EntityFrameworkCore;
using Stock.App.StockItems;
using Stock.Infrastructure.Common;

namespace Stock.Infrastructure.StockItems
{
    internal class StockItemLookup(StockDbContext context) : IStockItemLookup
    {
        public Task<Guid> FindBySkuAndVariant(string sku, string variantId, CancellationToken cancellationToken)
            => context.StockItems
                .AsNoTracking()
                .Where(x => x.Sku == sku && x.VariantId == variantId)
                .Take(1)
                .Select(res => res.Id)
                .SingleAsync(cancellationToken);
    }
}
