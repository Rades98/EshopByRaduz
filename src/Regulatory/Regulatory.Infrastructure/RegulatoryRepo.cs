using Microsoft.EntityFrameworkCore;
using Regulatory.Domain;
using Regulatory.Domain.VatRules;
using Regulatory.Infrastructure.Common;

namespace Regulatory.Infrastructure
{
    internal class RegulatoryRepo(RegulatoryDbContext context)
    {
        public Task<RegulatoryAggregate> GetAsync(Guid Id, CancellationToken cancellationToken)
            => context.Regulatories
                .AsNoTracking()
                .Where(r => r.Id == Id)
                .Select(r =>
                    RegulatoryAggregate.Rehydrate(
                        r.Id,
                        r.ProductGroup,
                        (r.VatRules.Select(x =>
                            VatRuleModel.Rehydrate(
                        ))).ToList()))
                .FirstOrDefaultAsync(cancellationToken)!;
    }
}
