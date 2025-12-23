using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing
{
    public interface IPricingRepo
    {
        Task<PricingAggregate> GetAsync(Guid priceGroupId, CancellationToken cancellationToken);

        Task<bool> AddAsync(PricingAggregate aggregate, CancellationToken cancellationToken);

        Task<bool> SaveAsync(PricingAggregate aggregate, CancellationToken cancellationToken);
    }
}
