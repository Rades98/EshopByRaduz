using DomainContracts;

namespace Pricing.Domain.Pricing.PriceItem
{
    public sealed class PriceItemModel
    {
        public string Sku { get; }

        public string VariantId { get; }

        public MoneyValueObject Price { get; }

        public PriceType PriceType { get; }

        public DateTime ValidFrom { get; }

        public DateTime? ValidTo { get; }

        public bool IsApplicable { get; }

        private PriceItemModel(
            string sku,
            string variantId,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo,
            bool isApplicable)
        {
            Sku = sku;
            VariantId = variantId;
            Price = price;
            PriceType = priceType;
            ValidFrom = validFrom;
            ValidTo = validTo;
            IsApplicable = isApplicable;
        }

        public static PriceItemModel CreateInapplicable(
            string sku,
            string variantId,
            DateTime validFrom)
        {
            return new PriceItemModel(
                sku,
                variantId,
                MoneyValueObject.Create(new(0, "CZK")).Value!,
                PriceType.Standard,
                validFrom,
                null,
                isApplicable: false);
        }

        public bool IsValid(DateTime at) =>
            IsApplicable && ValidFrom <= at && (ValidTo == null || at <= ValidTo);

        public static PriceItemModel Rehydrate(
            string sku,
            string variantId,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo,
            bool isApplicable)
        {
            return new PriceItemModel(
                sku,
                variantId,
                price,
                priceType,
                validFrom,
                validTo,
                isApplicable);
        }
    }
}
