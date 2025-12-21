using System.Collections.ObjectModel;

namespace Pricing.App.Pricing
{
    public interface IPricingLookup
    {
        Task<ReadOnlyCollection<Guid>> GetPriceIdsForProducts(string sku, string variantId, CancellationToken cancellationToken);
    }
}
