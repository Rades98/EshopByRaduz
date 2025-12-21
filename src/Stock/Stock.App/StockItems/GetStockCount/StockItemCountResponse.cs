namespace Stock.App.StockItems.GetStockCount
{
    public sealed record StockItemCountResponse(string Sku, string VariantId, int Amount);
}
