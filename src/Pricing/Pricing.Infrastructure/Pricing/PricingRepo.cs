using Database.SQL;
using Microsoft.EntityFrameworkCore;
using Pricing.App.Pricing;
using Pricing.Domain.Pricing;
using Pricing.Domain.Pricing.PriceItem;
using Pricing.Infrastructure.Common;
using Pricing.Infrastructure.Pricing.PriceGroup;
using Pricing.Infrastructure.Pricing.PriceItem;

namespace Pricing.Infrastructure.Pricing
{
    internal class PricingRepo(PricingDbContext context) : IPricingRepo
    {
        public Task<bool> AddAsync(PricingAggregate aggregate, CancellationToken cancellationToken)
            => context.AddWithinTransaction<PriceGroupEntity, PricingDbContext>(async context =>
            {
                var entity = new PriceGroupEntity
                {
                    Id = aggregate.GroupId,
                    Sku = aggregate.Sku,
                    VariantId = aggregate.VariantId,
                };

                foreach (var price in aggregate.Prices)
                {
                    var item = new PriceItemEntity
                    {
                        Price = price.Price.Amount,
                        CurrencyCode = price.Price.CurrencyCode,
                        PriceType = price.PriceType,
                        ValidFrom = price.ValidFrom,
                        ValidTo = price.ValidTo,
                    };

                    entity.Items.Add(item);
                }

                return await context.PriceGroups.AddAsync(entity, cancellationToken);

            }, cancellationToken);

        public Task<PricingAggregate> GetAsync(Guid priceGroupId, CancellationToken cancellationToken)
            => context.PriceGroups.AsNoTracking()
                .Include(pg => pg.Items)
                .Where(pg => pg.Id == priceGroupId)
                .Select(pg => PricingAggregate.Rehydrate(
                    pg.Id,
                    pg.Sku,
                    pg.VariantId,
                    pg.Items.Select(item => PriceItemModel.Rehydrate(
                        item.Id,
                        MoneyValueObject.Create(item.Price, item.CurrencyCode).Value!,
                        item.PriceType,
                        item.ValidFrom,
                        item.ValidTo)).ToList()))
                .SingleAsync(cancellationToken);

        public Task<bool> SaveAsync(PricingAggregate aggregate, CancellationToken cancellationToken)
            => context.UpdateWithinTransaction<PriceGroupEntity, PricingDbContext>(async context =>
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                var entity = await context.PriceGroups
                    .Include(pg => pg.Items)
                    .SingleAsync(pg => pg.Id == aggregate.GroupId, cancellationToken);

                foreach (var price in aggregate.Prices)
                {
                    var existingItem = entity.Items.FirstOrDefault(i => i.Id == price.Id);

                    if (existingItem is not null)
                    {
                        existingItem.ValidTo = price.ValidTo;
                    }
                    else
                    {
                        var newItem = new PriceItemEntity
                        {
                            Price = price.Price.Amount,
                            CurrencyCode = price.Price.CurrencyCode,
                            PriceType = price.PriceType,
                            ValidFrom = price.ValidFrom,
                            ValidTo = price.ValidTo,
                        };
                        entity.Items.Add(newItem);
                    }
                }
                context.PriceGroups.Update(entity);

            }, cancellationToken);
    }
}
