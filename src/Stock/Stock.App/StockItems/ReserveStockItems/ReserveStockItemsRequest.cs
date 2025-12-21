using System.Collections.ObjectModel;

namespace Stock.App.StockItems.ReserveStockUnits
{
    public sealed record ReserveStockItemsRequest(ReadOnlyCollection<ReserveStockItemRequest> Items, Guid CheckoutReference);

    public sealed record ReserveStockItemRequest(string Sku, string VariantId, int Amount);
}
