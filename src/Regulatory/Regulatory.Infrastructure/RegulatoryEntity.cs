using Regulatory.Infrastructure.VatCountry;
using System.Collections.ObjectModel;

namespace Regulatory.Infrastructure
{
    public class RegulatoryEntity
    {
        public Guid Id { get; set; }

        public required string ProductGroup { get; set; }

        public required string CountryCode { get; set; }

        public bool IsActive { get; set; }

        public Collection<VatRuleEntity> VatRules { get; } = [];
    }
}
