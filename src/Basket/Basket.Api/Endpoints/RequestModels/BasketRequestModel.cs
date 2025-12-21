namespace Basket.Api.Endpoints.RequestModels
{
    public sealed record BasketRequestModel(List<BasketItemRequestModel> Items);

    public sealed record BasketItemRequestModel(string Sku, string Variant, int Quantity);
}
