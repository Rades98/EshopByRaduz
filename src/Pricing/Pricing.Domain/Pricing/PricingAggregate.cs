using DomainContracts;
using DomainContracts.Events.Pricing;
using DomainObjects;
using Pricing.Domain.Pricing.PriceItem;

namespace Pricing.Domain.Pricing
{
    public class PricingAggregate : AggregateRoot
    {
        private List<PriceItemModel> _prices = [];

        private string _sku;
        private string _variantId;

        public string Sku => _sku;

        public string VariantId => _variantId;

        public Guid GroupId { get; private set; }

        private PricingAggregate(Guid id, string sku, string variant, IReadOnlyList<PriceItemModel> prices)
        {
            _prices = [.. prices];
            _sku = sku;
            _variantId = variant;
            GroupId = id;
        }

        public IReadOnlyList<PriceItemModel> Prices => _prices;

        public static PricingAggregate Create(Guid id, string sku, string variant)
            => new(id, sku, variant, []);

        public Result<PriceItemModel> AddPrice(PriceType priceType, MoneyValueObject moneyValue, DateTime validFrom, DateTime? validTo)
        {
            var newPrice = PriceItemModel.Rehydrate(moneyValue, priceType, validFrom, validTo);

            var overlap = _prices.Any(p =>
                p.PriceType == priceType &&
                p.PriceType != PriceType.Promo &&
                (
                    (p.ValidTo == null) ||
                    (validTo == null) ||
                    (p.ValidTo != null && validTo != null &&
                     p.ValidFrom <= validTo && validFrom <= p.ValidTo)
                )
            );

            if (overlap)
            {
                return Result<PriceItemModel>.Failure("PRICE_ERROR_OVERLAP");
            }

            _prices.Add(newPrice);

            AddDomainEvent(new PriceItemAddedEvent(
                Sku,
                VariantId,
                priceType.ToString(),
                moneyValue.Amount,
                moneyValue.CurrencyCode,
                "Pricing"));

            return Result<PriceItemModel>.Success(newPrice);
        }

        public bool HasValidPrice(PriceType priceType, string currencyCode, DateTime at)
        {
            return _prices.Any(p =>
                p.PriceType == priceType &&
                p.IsValid(at) &&
                p.Price.CurrencyCode == currencyCode);
        }

        public Result<PriceItemModel> UpdatePrice(
            PriceType priceType,
            MoneyValueObject newPrice,
            DateTime validFrom,
            DateTime? validTo = null)
        {
            var updated = PriceItemModel.Rehydrate(newPrice, priceType, validFrom, validTo);

            var overlappingPrices = _prices
                .Where(p => p.PriceType == priceType && p.PriceType != PriceType.Promo && p.Overlaps(updated))
                .ToList();

            foreach (var old in overlappingPrices)
            {
                old.Invalidate();
            }

            _prices.Add(updated);

            AddDomainEvent(new PriceItemAddedEvent(
                Sku,
                VariantId,
                priceType.ToString(),
                newPrice.Amount,
                newPrice.CurrencyCode,
                "Pricing"));

            return Result<PriceItemModel>.Success(updated);
        }

        public Result<PriceItemModel> GetPrice(
            PriceType requestedType,
            string currencyCode,
            DateTime at)
        {
            var price = _prices
                .Where(p =>
                    p.PriceType == requestedType &&
                    p.IsValid(at) &&
                    p.Price.CurrencyCode == currencyCode)
                .OrderByDescending(p => p.ValidFrom)
                    .ThenByDescending(p => p.ValidTo)
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
                        p.PriceType == PriceType.Standard &&
                        p.IsValid(at))
                    .OrderByDescending(p => p.ValidFrom)
                        .ThenByDescending(p => p.ValidTo)
                    .FirstOrDefault();

                if (price != null)
                {
                    return Result<PriceItemModel>.Success(price);
                }
            }

            return Result<PriceItemModel>.Failure("PRICE_ERROR_NOT_FOUND");
        }

        public static PricingAggregate Rehydrate(Guid id, string sku, string variant, IReadOnlyList<PriceItemModel> prices)
            => new(id, sku, variant, prices ?? []);
    }
}
