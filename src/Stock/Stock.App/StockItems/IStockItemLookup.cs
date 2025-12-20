namespace Stock.App.StockItems
{
    public interface IStockItemLookup
    {
        public Task<Guid> FindBySkuAndVariant(string sku, string variantId, CancellationToken cancellationToken);
    }
}
