using Database.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pricing.App.Pricing;
using Pricing.Domain.Pricing;
using Pricing.Domain.Pricing.PriceItem;
using Pricing.Infrastructure.Common;
using Pricing.Infrastructure.Pricing.PriceItem;
using System.Collections.ObjectModel;

namespace Pricing.Infrastructure.Pricing
{
    internal class PricingRepo(PricingDbContext context) : IPricingRepo
    {
        public Task<bool> AddAsync(PricingAggregate aggregate, CancellationToken cancellationToken)
            => context.AddManyWithinTransaction<PriceItemEntity, PricingDbContext>(async context =>
            {
                var addedEntries = new List<EntityEntry<PriceItemEntity>>();

                foreach (var price in aggregate.Prices)
                {
                    var entity = new PriceItemEntity
                    {
                        Id = Guid.NewGuid(),
                        Sku = price.Sku,
                        VariantId = price.VariantId,
                        Price = price.Price.Amount,
                        CurrencyCode = price.Price.CurrencyCode,
                        PriceType = price.PriceType,
                        ValidFrom = price.ValidFrom,
                        ValidTo = price.ValidTo,
                        IsApplicable = price.IsApplicable
                    };

                    var entry = await context.AddAsync(entity, cancellationToken);
                    addedEntries.Add(entry);
                }

                return addedEntries;

            }, cancellationToken);

        public async Task<PricingAggregate?> GetAsync(ReadOnlyCollection<Guid> priceItemIds, CancellationToken cancellationToken)
            => PricingAggregate.Rehydrate(await context.PriceItems
                .AsNoTracking()
                .Where(x => priceItemIds.Contains(x.Id))
                .Select(res =>
                    PriceItemModel.Rehydrate(
                        res.Sku,
                        res.VariantId,
                        MoneyValueObject.Create(new(res.Price, res.CurrencyCode ?? "UNSET")).Value!,
                        res.PriceType,
                        res.ValidFrom,
                        res.ValidTo,
                        res.IsApplicable))
                .ToListAsync(cancellationToken)!);

        public Task SaveAsync(PricingAggregate aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
