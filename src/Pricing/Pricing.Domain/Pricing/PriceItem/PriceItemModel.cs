using DomainContracts;

namespace Pricing.Domain.Pricing.PriceItem
{
    public sealed class PriceItemModel
    {
        public MoneyValueObject Price { get; }

        public PriceType PriceType { get; }

        public DateTime ValidFrom { get; }

        public DateTime? ValidTo { get; private set; }

        private PriceItemModel(
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            Price = price;
            PriceType = priceType;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public bool IsValid(DateTime at) =>
            ValidFrom <= at && (ValidTo == null || at <= ValidTo);

        public bool Overlaps(PriceItemModel other)
            => (other.ValidTo == null) ||
            (ValidTo == null) ||
            (other.ValidFrom <= ValidTo && ValidFrom <= other.ValidTo);

        public void Invalidate()
        {
            ValidTo = DateTime.UtcNow;
        }

        public static PriceItemModel Rehydrate(
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            return new PriceItemModel(
                price,
                priceType,
                validFrom,
                validTo);
        }
    }
}
