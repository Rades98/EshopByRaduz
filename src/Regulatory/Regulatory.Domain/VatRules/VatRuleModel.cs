namespace Regulatory.Domain.VatRules
{
    public class VatRuleModel
    {
        public Guid Id { get; private set; }

        public decimal VatRate { get; private set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        private VatRuleModel(Guid id, decimal vatRate, DateTime validFrom, DateTime? validTo)
        {
            Id = id;
            VatRate = vatRate;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public bool IsValid(DateTime at) =>
            ValidFrom <= at && (ValidTo == null || at <= ValidTo);

        public bool Overlaps(VatRuleModel other)
            => (other.ValidTo == null) ||
            (ValidTo == null) ||
            (other.ValidFrom <= ValidTo && ValidFrom <= other.ValidTo);

        public void Invalidate()
        {
            ValidTo = DateTime.UtcNow;
        }

        public static VatRuleModel Rehydrate(Guid id, decimal vatRate, DateTime validFrom, DateTime? validTo)
        {
            return new(id, vatRate, validFrom, validTo);
        }

        internal static VatRuleModel Create(Guid id, decimal vatRate, DateTime validFrom, DateTime? validTo)
        {
            return new(id, vatRate, validFrom, validTo);
        }
    }
}
