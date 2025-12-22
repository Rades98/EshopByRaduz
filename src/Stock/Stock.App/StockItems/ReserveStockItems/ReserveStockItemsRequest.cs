using System.Collections.ObjectModel;

namespace Stock.App.StockItems.ReserveStockItems
{
    public sealed record ReserveStockItemsRequest(ReadOnlyCollection<ReserveStockItemRequest> Items, Guid CheckoutReference);

    public sealed record ReserveStockItemRequest(string Sku, string VariantId, int Amount);
}
