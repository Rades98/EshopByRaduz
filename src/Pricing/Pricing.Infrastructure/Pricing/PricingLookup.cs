using Microsoft.EntityFrameworkCore;
using Pricing.App.Pricing;
using Pricing.Infrastructure.Common;

namespace Pricing.Infrastructure.Pricing
{
    internal class PricingLookup(PricingDbContext context) : IPricingLookup
    {
        public Task<Guid> GetPriceGroupIdforProduct(string sku, string variantId, CancellationToken cancellationToken)
            => context.PriceGroups
                .AsNoTracking()
                .Where(x => x.Sku == sku && x.VariantId == variantId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
