using Grpc.Core;
using MediatR;
using Pricing.App.Pricing.GetPricesForProducts;

namespace Pricing.Grpc.Services
{
    public class PricingGrpcService(IMediator mediator) : PricingService.PricingServiceBase
    {
        public async override Task<GetComputedPricesForProductsResponse> GetPrices(GetPricesRequest request, ServerCallContext context)
        {
            GetPricesForProductsRequest mediatorRequest =
                new GetPricesForProductsRequest(
                    ConvertPriceType(request.PriceType),
                    request.CurrencyCode,
                    request.Items.Select(item =>
                        new GetPriceForProductsRequest(
                            item.Sku,
                            item.VariantId,
                            item.Quantity)).ToList().AsReadOnly());

            var res = await mediator.Send(new GetPricesForProductsQuery(mediatorRequest), context.CancellationToken);

            return res.Match(res =>
            {
                return new GetComputedPricesForProductsResponse
                {
                    Success = true,
                    Items =
                    {
                        res.Items.Select(item => new PricePerItemResult
                        {
                            Sku = item.Sku,
                            VariantId = item.VariantId,
                            TotalPrice = new Money
                            {
                                Amount = $"{item.TotalPrice.Amount}",
                                CurrencyCode = item.TotalPrice.CurrencyCode
                            },
                            UnitPrice = new Money
                            {
                                Amount = $"{item.UnitPrice.Amount}",
                                CurrencyCode = item.UnitPrice.CurrencyCode
                            }

                        })
                    },
                    Total = new Money
                    {
                        Amount = $"{res.Total.Amount}",
                        CurrencyCode = res.Total.CurrencyCode
                    }
                };
            },
            err =>
            {
                return new GetComputedPricesForProductsResponse
                {
                    Success = false,
                    Error = err
                };
            });
        }

        private static DomainContracts.PriceType ConvertPriceType(PriceType priceType)
        {
            return priceType switch
            {
                PriceType.Standard => DomainContracts.PriceType.Standard,
                PriceType.Promo => DomainContracts.PriceType.Promo,
                PriceType.B2B => DomainContracts.PriceType.B2B,
                _ => throw new ArgumentOutOfRangeException(nameof(priceType), $"Not expected price type value: {priceType}"),
            };
        }
    }
}
