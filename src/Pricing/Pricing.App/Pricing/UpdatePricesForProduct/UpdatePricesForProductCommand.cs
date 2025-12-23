using Database.Contracts;
using InOutbox.Orchestrator.Repos;
using Mediator.Request.Command;
using MediatR;
using OneOf;
using Pricing.Domain.Pricing;

namespace Pricing.App.Pricing.UpdatePricesForProduct
{
    public sealed record UpdatePricesForProductCommand(UpdatePricesForProductRequest Request) : ICommand<OneOf<Unit, string>>
    {
        internal sealed class UpdatePricesForProductCommandHandler(
            IPricingLookup lookup,
            IPricingRepo repo,
            IOutboxRepo outboxRepo,
            IUnitOfWork uow) : ICommandHandler<UpdatePricesForProductCommand, OneOf<Unit, string>>
        {
            public async Task<OneOf<Unit, string>> Handle(UpdatePricesForProductCommand command, CancellationToken cancellationToken)
            {
                var pricingId = await lookup.GetPriceGroupIdforProduct(command.Request.Sku, command.Request.Variant, cancellationToken);

                if (pricingId == Guid.Empty)
                {
                    return "ERROR_PRICE_GROUP_NOT_FOUND";
                }

                var aggreagete = await repo.GetAsync(pricingId, cancellationToken);

                foreach (var item in command.Request.Items)
                {
                    if (aggreagete.HasValidPrice(item.PriceType, item.CurrencyCode, DateTime.UtcNow))
                    {
                        var money = MoneyValueObject.Create(item.Price, item.CurrencyCode);

                        if (!money.IsSuccess)
                        {
                            return money.Error!;
                        }

                        // TODO FIX
                        aggreagete.UpdatePrice(item.PriceType, money.Value!, item.ValidFrom, item.ValidTo);
                    }
                    else
                    {
                        var money = MoneyValueObject.Create(item.Price, item.CurrencyCode);

                        if (!money.IsSuccess)
                        {
                            return money.Error!;
                        }

                        var res = aggreagete.AddPrice(item.PriceType, money.Value!, item.ValidFrom, item.ValidTo);

                        if (!res.IsSuccess)
                        {
                            return res.Error!;
                        }
                    }
                }

                await uow.BeginTransactionAsync(cancellationToken);

                try
                {

                    await outboxRepo.AddRangeAsync(aggreagete.DomainEvents, cancellationToken);

                    aggreagete.ClearDomainEvents();

                    var updateResult = await repo.SaveAsync(aggreagete, cancellationToken);

                    await uow.CommitTransactionAsync(cancellationToken);

                    if (updateResult)
                    {
                        return Unit.Value;
                    }
                    else
                    {
                        return "ERROR_COULD_NOT_SAVE";
                    }


                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(cancellationToken);
                    return "ERROR_COULD_NOT_SAVE";
                }
            }
        }
    }
}
