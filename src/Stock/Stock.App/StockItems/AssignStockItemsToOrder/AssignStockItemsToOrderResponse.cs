using System.Collections.ObjectModel;

namespace Stock.App.StockItems.AssignStockItemsToOrder
{
    public record AssignStockItemsToOrderResponse(bool Success, ReadOnlyCollection<AssignStockItemToOrderResponse>? Items = null);
    public record AssignStockItemToOrderResponse(string Sku, string VariantId, ReadOnlyCollection<string> SerialNumbers);
}
