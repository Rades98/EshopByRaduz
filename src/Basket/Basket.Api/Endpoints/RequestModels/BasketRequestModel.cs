using System.Collections.ObjectModel;

namespace Basket.Api.Endpoints.RequestModels
{
    internal sealed record BasketRequestModel(ReadOnlyCollection<BasketItemRequestModel> Items);

    internal sealed record BasketItemRequestModel(string Sku, string Variant, int Quantity);
}
