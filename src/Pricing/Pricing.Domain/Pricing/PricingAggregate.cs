using DomainContracts;
using DomainObjects;
using Pricing.Domain.Pricing.PriceItem;

namespace Pricing.Domain.Pricing
{
    public class PricingAggregate : AggregateRoot
    {
        private readonly IReadOnlyList<PriceItemModel> _prices;

        private PricingAggregate(IReadOnlyList<PriceItemModel> prices)
        {
            _prices = prices;
        }

        public Result<PriceItemModel> GetPrice(
            string sku,
            string? variantId,
            PriceType requestedType,
            string currencyCode,
            DateTime at)
        {
            var price = _prices
                .Where(p =>
                    p.Sku == sku &&
                    p.VariantId == variantId &&
                    p.PriceType == requestedType &&
                    p.IsValid(at) &&
                    p.Price.CurrencyCode == currencyCode)
                .OrderByDescending(p => p.ValidFrom)
                .FirstOrDefault();

            if (price != null)
            {
                return Result<PriceItemModel>.Success(price);
            }

            // Fallback to Standard price
            if (requestedType != PriceType.Standard)
            {
                price = _prices
                    .Where(p =>
                        p.Sku == sku &&
                        p.VariantId == variantId &&
                        p.PriceType == PriceType.Standard &&
                        p.IsValid(at))
                    .OrderByDescending(p => p.ValidFrom)
                    .FirstOrDefault();

                if (price != null)
                {
                    return Result<PriceItemModel>.Success(price);
                }
            }

            return Result<PriceItemModel>.Failure("PRICE_ERROR_NOT_FOUND");
        }

        public static PricingAggregate Rehydrate(IReadOnlyList<PriceItemModel> prices)
            => new(prices);
    }
}
