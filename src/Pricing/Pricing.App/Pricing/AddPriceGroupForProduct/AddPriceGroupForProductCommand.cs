using Mediator.Request.Command;
using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing.AddPriceGroupForProduct
{
    public sealed record AddPriceGroupForProductCommand(string Sku, string Variant) : ICommand<bool>
    {
        internal sealed class AddPriceGroupForProductCommandHandler(
            IPricingRepo repo,
            IPricingLookup lookup) : ICommandHandler<AddPriceGroupForProductCommand, bool>
        {
            public async Task<bool> Handle(AddPriceGroupForProductCommand request, CancellationToken cancellationToken)
            {
                var existence = await lookup.GetPriceGroupIdforProduct(request.Sku, request.Variant, cancellationToken);

                if (existence != Guid.Empty)
                {
                    return true;
                }

                var aggregate = PricingAggregate.Create(Guid.NewGuid(), request.Sku, request.Variant);

                return await repo.AddAsync(aggregate, cancellationToken);
            }
        }
    }
}
