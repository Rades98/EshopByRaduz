using Microsoft.EntityFrameworkCore;
using Pricing.App.Pricing;
using Pricing.Domain.Pricing;
using Pricing.Domain.Pricing.PriceItem;
using Pricing.Infrastructure.Common;
using System.Collections.ObjectModel;

namespace Pricing.Infrastructure.Pricing
{
    internal class PricingRepo(PricingDbContext context) : IPricingRepo
    {
        public Task AddAsync(PricingAggregate aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();

        public async Task<PricingAggregate?> GetAsync(ReadOnlyCollection<Guid> priceItemIds, CancellationToken cancellationToken)
            => PricingAggregate.Rehydrate(await context.PriceItems
                .AsNoTracking()
                .Where(x => priceItemIds.Contains(x.Id))
                .Select(res =>
                    PriceItemModel.Rehydrate(
                        res.Sku,
                        res.VariantId,
                        MoneyValueObject.Create(new(res.Price, res.CurrencyCode)).Value!,
                        res.PriceType,
                        res.ValidFrom,
                        res.ValidTo))
                .ToListAsync(cancellationToken)!);

        public Task SaveAsync(PricingAggregate aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
