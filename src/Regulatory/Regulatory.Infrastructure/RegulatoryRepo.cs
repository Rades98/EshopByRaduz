using Database.SQL;
using Microsoft.EntityFrameworkCore;
using Regulatory.Domain;
using Regulatory.Domain.VatRules;
using Regulatory.Infrastructure.Common;
using Regulatory.Infrastructure.VatCountry;

namespace Regulatory.Infrastructure
{
    internal class RegulatoryRepo(RegulatoryDbContext context)
    {
        public Task<VatAggregate> GetAsync(Guid Id, CancellationToken cancellationToken)
            => context.Regulatories
                .AsNoTracking()
                .Where(r => r.Id == Id)
                .Select(r =>
                    VatAggregate.Rehydrate(
                        r.Id,
                        r.ProductGroup,
                        (r.VatRules.Select(x =>
                            VatRuleModel.Rehydrate(x.Id, x.VatRate, x.ValidFrom, x.ValidTo
                        ))).ToList(),
                        CountryValueObject.Create(r.CountryCode).Value!,
                        r.IsActive))
                .SingleAsync(cancellationToken)!;

        public Task<bool> SaveAsync(VatAggregate aggregate, CancellationToken cancellationToken)
            => context.UpdateWithinTransaction<RegulatoryEntity, RegulatoryDbContext>(async context =>
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                var entity = await context.Regulatories
                    .Include(pg => pg.VatRules)
                    .SingleAsync(pg => pg.Id == aggregate.Id, cancellationToken);

                foreach (var vatRule in aggregate.Vats)
                {
                    var existingItem = entity.VatRules.FirstOrDefault(i => i.Id == vatRule.Id);

                    if (existingItem is not null)
                    {
                        existingItem.ValidTo = vatRule.ValidTo;
                    }
                    else
                    {
                        var newItem = new VatRuleEntity
                        {
                            ValidFrom = vatRule.ValidFrom,
                            ValidTo = vatRule.ValidTo,
                            RegulatoryId = aggregate.Id,
                            VatRate = vatRule.VatRate,
                        };

                        entity.VatRules.Add(newItem);
                    }
                }

                context.Regulatories.Update(entity);

            }, cancellationToken);

        public Task<bool> AddAsync(VatAggregate aggregate, CancellationToken cancellationToken)
            => context.AddWithinTransaction<RegulatoryEntity, RegulatoryDbContext>(async context =>
            {
                var entity = new RegulatoryEntity
                {
                    Id = aggregate.Id,
                    CountryCode = aggregate.Country.Value,
                    IsActive = aggregate.IsActive,
                    ProductGroup = aggregate.ProductGroup,
                };

                foreach (var vatRule in aggregate.Vats)
                {
                    var newItem = new VatRuleEntity
                    {
                        ValidFrom = vatRule.ValidFrom,
                        ValidTo = vatRule.ValidTo,
                        RegulatoryId = aggregate.Id,
                        VatRate = vatRule.VatRate,
                    };

                    entity.VatRules.Add(newItem);
                }

                return await context.Regulatories.AddAsync(entity, cancellationToken);

            }, cancellationToken);
    }
}
