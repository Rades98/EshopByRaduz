using DomainContracts;

namespace Pricing.Domain.Pricing.PriceItem
{
    public sealed class PriceItemModel
    {
        public MoneyValueObject Price { get; }

        public PriceType PriceType { get; }

        public DateTime ValidFrom { get; }

        public DateTime? ValidTo { get; private set; }

        public Guid Id { get; private set; }

        private PriceItemModel(
            Guid id,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            Id = id;
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
            Guid id,
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            return new PriceItemModel(
                id,
                price,
                priceType,
                validFrom,
                validTo);
        }

        // TODO RESULT - validate price etc
        internal static PriceItemModel Create(
            MoneyValueObject price,
            PriceType priceType,
            DateTime validFrom,
            DateTime? validTo)
        {
            return new PriceItemModel(
                Guid.NewGuid(),
                price,
                priceType,
                validFrom,
                validTo);
        }
    }
}
