using DomainObjects;
using Regulatory.Domain.VatRules;
using System.Collections.ObjectModel;

namespace Regulatory.Domain
{
    public class VatAggregate : AggregateRoot
    {
        private List<VatRuleModel> _vatRules = [];

        public string ProductGroup { get; private set; }

        public CountryValueObject Country { get; private set; }

        public Guid Id { get; private set; }

        public bool IsActive { get; private set; }

        public ReadOnlyCollection<VatRuleModel> Vats => new(_vatRules);

        private VatAggregate(Guid id, string productCode, IReadOnlyList<VatRuleModel> vatRules, CountryValueObject country, bool isActive)
        {
            Id = id;
            ProductGroup = productCode;
            _vatRules = [.. vatRules];
            Country = country;
            IsActive = isActive;
        }

        public static VatAggregate Rehydrate(Guid id, string productCode, IReadOnlyList<VatRuleModel> vatRules, CountryValueObject country, bool isActive)
            => new(id, productCode, vatRules, country, isActive);

        // TODO RESULT
        public static VatAggregate Create(Guid id, string productCode, CountryValueObject country, bool isActive)
            => new(id, productCode, [], country, isActive);
    }
}
