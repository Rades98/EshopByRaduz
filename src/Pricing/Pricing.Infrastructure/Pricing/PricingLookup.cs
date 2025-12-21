using Microsoft.EntityFrameworkCore;
using Pricing.App.Pricing;
using Pricing.Infrastructure.Common;
using System.Collections.ObjectModel;

namespace Pricing.Infrastructure.Pricing
{
    internal class PricingLookup(PricingDbContext context) : IPricingLookup
    {
        public async Task<ReadOnlyCollection<Guid>> GetPriceIdsForProducts(string sku, string variantId, CancellationToken cancellationToken)
            => (await context.PriceItems
                .AsNoTracking()
                .Where(x => x.Sku == sku && x.VariantId == variantId)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken)).AsReadOnly();
    }
}
