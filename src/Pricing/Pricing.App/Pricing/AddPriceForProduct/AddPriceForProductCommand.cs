using Mediator.Request.Command;
using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing.AddPriceForProduct
{
    public sealed record AddPriceForProductCommand(string Sku, string Variant) : ICommand<bool>
    {
        internal sealed class AddPriceForProductCommandHandler(IPricingRepo repo, IPricingLookup lookup) : ICommandHandler<AddPriceForProductCommand, bool>
        {
            public async Task<bool> Handle(AddPriceForProductCommand request, CancellationToken cancellationToken)
            {
                var existence = await lookup.GetPriceIdsForProducts(request.Sku, request.Variant, cancellationToken);

                if (existence.Count > 0)
                {
                    return true;
                }

                var aggregate = PricingAggregate.Create(request.Sku, request.Variant).AddInapplicableItem(DateTime.UtcNow);

                return await repo.AddAsync(aggregate, cancellationToken);
            }
        }
    }
}
