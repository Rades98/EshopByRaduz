namespace Regulatory.Infrastructure.VatCountry
{
    public class VatRuleEntity
    {
        public Guid Id { get; set; }

        public Guid RegulatoryId { get; set; }

        public RegulatoryEntity Regulatory { get; set; } = null!;

        public required string CountryCode { get; set; }

        public decimal VatRate { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
