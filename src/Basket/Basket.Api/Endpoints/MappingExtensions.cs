using Basket.Api.Dtos;
using Basket.Api.Endpoints.RequestModels;
using Basket.Api.Services;
using Pricing.Grpc;

namespace Basket.Api.Endpoints
{
    internal static class MappingExtensions
    {
        public static async Task<BasketDto> MapBasket(this BasketRequestModel requestModel, PricingGrpcService pricingGrpcService, CancellationToken ct)
        {
            var request = new GetPricesRequest()
            {
                PriceType = PriceType.Standard,
                CurrencyCode = "CZK"
            };

            foreach (var item in requestModel.Items)
            {
                request.Items.Add(new GetPriceRequestItem()
                {
                    Sku = item.Sku,
                    VariantId = item.Variant,
                    Quantity = item.Quantity
                });
            }

            var stockPrices = await pricingGrpcService.GetStockCountAsync(request, ct);

            var basketItems = new List<BasketItemDto>();

            foreach (var item in requestModel.Items)
            {
                var priceInfo = stockPrices.Items.FirstOrDefault(x => x.Sku == item.Sku && x.VariantId == item.Variant)!;
                basketItems.Add(new BasketItemDto(item.Sku, item.Variant, item.Quantity, priceInfo.TotalPrice.Amount, priceInfo.UnitPrice.Amount));
            }

            return new BasketDto(basketItems, stockPrices.Total.CurrencyCode, stockPrices.Total.Amount);
        }
    }
}
