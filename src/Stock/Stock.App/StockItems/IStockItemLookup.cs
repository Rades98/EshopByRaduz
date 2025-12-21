using System.Collections.ObjectModel;

namespace Stock.App.StockItems
{
    public interface IStockItemLookup
    {
        public Task<Guid> FindBySkuAndVariant(string sku, string variantId, CancellationToken cancellationToken);

        public Task<ReadOnlyCollection<Guid>> FindByCheckoutReference(Guid checkoutReference, CancellationToken cancellationToken);
    }
}
