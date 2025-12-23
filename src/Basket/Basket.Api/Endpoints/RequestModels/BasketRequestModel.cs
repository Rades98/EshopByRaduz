namespace Basket.Api.Endpoints.RequestModels
{
    internal sealed record BasketRequestModel(List<BasketItemRequestModel> Items);

    internal sealed record BasketItemRequestModel(string Sku, string Variant, int Quantity);
}
