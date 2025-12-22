using DomainContracts;
using DomainObjects;
using Pricing.Domain.Pricing.PriceItem;

namespace Pricing.Domain.Pricing
{
    public class PricingAggregate : AggregateRoot
    {
        private List<PriceItemModel> _prices = [];

        private string _sku;
        private string _variantId;

        private PricingAggregate(IReadOnlyList<PriceItemModel> prices)
        {
            _prices = [.. prices];
            _sku = prices.Count > 0 ? prices[0].Sku : string.Empty;
            _variantId = prices.Count > 0 ? prices[0].VariantId : string.Empty;
        }

        public IReadOnlyList<PriceItemModel> Prices => _prices;

        public static PricingAggregate Create(string sku, string variant)
            => new([]) { _sku = sku, _variantId = variant };

        public PricingAggregate AddInapplicableItem(DateTime validFrom)
        {
            var item = PriceItemModel.CreateInapplicable(_sku, _variantId, validFrom);

            _prices.Add(item);

            return this;
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
            => new(prices ?? []);
    }
}
