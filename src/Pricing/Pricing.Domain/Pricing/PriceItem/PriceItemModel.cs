using DomainContracts;

namespace Pricing.Domain.Pricing.PriceItem
{
    public sealed class PriceItemModel
    {
        public string Sku { get; }

        public string? VariantId { get; }

        public MoneyValueObject Price { get; }

        public PriceType PriceType { get; }

        public DateTime ValidFrom { get; }

        public DateTime? ValidTo { get; }

        private PriceItemModel(
            string sku,
            string? variantId,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            Sku = sku;
            VariantId = variantId;
            Price = price;
            PriceType = priceType;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public bool IsValid(DateTime at) =>
            ValidFrom <= at && (ValidTo == null || at <= ValidTo);

        public static PriceItemModel Rehydrate(
            string sku,
            string? variantId,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            return new PriceItemModel(
                sku,
                variantId,
                price,
                priceType,
                validFrom,
                validTo);
        }
    }
}
