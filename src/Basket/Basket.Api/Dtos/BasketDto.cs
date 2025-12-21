namespace Basket.Api.Dtos
{
    public sealed record BasketDto(List<BasketItemDto> Items, string Currency, string Total);

    public sealed record BasketItemDto(string Sku, string Variant, int Quantity, string Price, string PricePerItem);
}
