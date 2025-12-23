using DomainObjects;
using Regulatory.Domain.VatRules;
using System.Collections.ObjectModel;

namespace Regulatory.Domain
{
    public class RegulatoryAggregate : AggregateRoot
    {
        private List<VatRuleModel> _vatRules = [];

        public string ProductGroup { get; private set; }

        public Guid Id { get; private set; }

        public ReadOnlyCollection<VatRuleModel> Vats => new(_vatRules);

        private RegulatoryAggregate(Guid id, string productCode, IReadOnlyList<VatRuleModel> vatRules)
        {
            Id = id;
            ProductGroup = productCode;
            _vatRules = [.. vatRules];
        }

        public static RegulatoryAggregate Rehydrate(Guid id, string productCode, IReadOnlyList<VatRuleModel> vatRules)
            => new(id, productCode, vatRules);

        public static RegulatoryAggregate Create(Guid id, string productCode)
            => new(id, productCode, []);
    }
}
