using Pricing.Domain.Pricing;
using System.Collections.ObjectModel;

namespace Pricing.App.Pricing
{
    public interface IPricingRepo
    {
        Task<PricingAggregate?> GetAsync(ReadOnlyCollection<Guid> priceItemIds, CancellationToken cancellationToken);

        Task<bool> AddAsync(PricingAggregate aggregate, CancellationToken cancellationToken);

        Task SaveAsync(PricingAggregate aggregate, CancellationToken cancellationToken);
    }
}
