namespace Basket.Api.Dtos
{
    internal sealed record BasketDto(List<BasketItemDto> Items, string Currency, string Total);

    internal sealed record BasketItemDto(string Sku, string Variant, int Quantity, string Price, string PricePerItem);
}
