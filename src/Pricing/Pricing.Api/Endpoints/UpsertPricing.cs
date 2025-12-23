using DomainContracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Pricing.App.Pricing.UpdatePricesForProduct;

namespace Pricing.Api.Endpoints
{
    internal static class UpsertPricing
    {
        internal static IEndpointRouteBuilder MapUpsertPricingEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("groups/{sku}/{variant}/prices",
                async (
                    [FromRoute] string sku,
                    [FromRoute] string variant,
                    [FromBody] UpdatePricesForProductRequestModel request,
                    [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                            {
                                var items = request.Items.Select(x =>
                                    new UpdatePricesForProductItemRequest(Enum.Parse<PriceType>(x.PriceType), x.Price, x.CurrencyCode, x.ValidFrom, x.ValidTo)).ToList().AsReadOnly();
                                var mdtrReq = new UpdatePricesForProductRequest(sku, variant, items);

                                var res = await mediator.Send<OneOf<Unit, string>>(new UpdatePricesForProductCommand(mdtrReq), cancellationToken);

                                return res.Match(
                                    success => Results.Ok(),
                                    error => Results.BadRequest(error));
                            })
            .AllowAnonymous()
            .WithName("UpsertPricing")
            .WithOpenApi();

            return app;
        }

        internal sealed record UpdatePricesForProductRequestModel(string Sku, string Variant, List<UpdatePricesForProductItemRequestModel> Items);

        internal sealed record UpdatePricesForProductItemRequestModel(string PriceType, decimal Price, string CurrencyCode, DateTime ValidFrom, DateTime? ValidTo = null);
    }
}
