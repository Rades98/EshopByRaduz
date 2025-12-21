using Mediator.Request.Query;
using OneOf;
using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing.GetPricesForProducts
{
    public record GetPricesForProductsQuery(GetPricesForProductsRequest Request) : IQuery<OneOf<GetPricesForProductsResponse, string>>
    {
        internal sealed class GetPricesForProductsQueryHandler(IPricingLookup pricingLookup, IPricingRepo pricingRepo)
            : IQueryHandler<GetPricesForProductsQuery, OneOf<GetPricesForProductsResponse, string>>
        {
            public async Task<OneOf<GetPricesForProductsResponse, string>> Handle(GetPricesForProductsQuery query, CancellationToken cancellationToken)
            {
                var priceResults = new List<PricePerItemResult>();
                var total = MoneyValueObject.Create((0m, query.Request.CurrencyCode)).Value!;

                foreach (var item in query.Request.Items)
                {
                    var pricingIds = await pricingLookup.GetPriceIdsForProducts(item.Sku, item.VariantId, cancellationToken);
                    var pricingAggregate = await pricingRepo.GetAsync(pricingIds, cancellationToken);

                    if (pricingAggregate is null)
                    {
                        continue;
                    }

                    var priceResult = pricingAggregate.GetPrice(
                        item.Sku,
                        item.VariantId,
                        query.Request.PriceType,
                        query.Request.CurrencyCode,
                        DateTime.UtcNow);

                    if (!priceResult.IsSuccess)
                    {
                        return priceResult.Error!;
                    }

                    var unitPrice = priceResult.Value!.Price;
                    var totalPrice = unitPrice.Multiply(item.Quantity);

                    priceResults.Add(new PricePerItemResult(
                        item.Sku,
                        item.VariantId ?? string.Empty,
                        unitPrice,
                        totalPrice));

                    total = MoneyValueObject.Create((total.Amount + totalPrice.Amount, total.CurrencyCode)).Value!;
                }

                return new GetPricesForProductsResponse(priceResults, total);
            }
        }
    }
}
