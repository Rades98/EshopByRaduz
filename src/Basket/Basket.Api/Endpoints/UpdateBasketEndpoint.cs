using Basket.Api.Endpoints.RequestModels;
using Basket.Api.Services;
using DomainContracts.Events.Basket;
using Kafka;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Endpoints
{
    internal static class UpdateBasketEndpoint
    {
        internal static IEndpointRouteBuilder MapUpdateBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/users/{userId:guid}/basket/",
                async (
                    [FromRoute] Guid userId,
                    [FromBody] BasketRequestModel req,
                    [FromServices] IConnectionMultiplexer redis,
                    [FromServices] IKafkaPublisher publisher,
                    [FromServices] StockGrpcService stockGrpcService,
                    [FromServices] PricingGrpcService pricingGrpcService,
                    CancellationToken ct) =>
                {
                    var db = redis.GetDatabase();
                    var basketKey = $"basket:{userId}";

                    var insufficientItems = await req.GetInvalidStockItems(stockGrpcService, ct);

                    if (insufficientItems.Count != 0)
                    {
                        return Results.UnprocessableEntity(new
                        {
                            Error = "INSUFFICIENT_STOCK",
                            Items = insufficientItems
                        });
                    }

                    var mappedWithPrices = await req.MapBasket(pricingGrpcService, ct);

                    await db.StringSetAsync(basketKey, JsonSerializer.Serialize(mappedWithPrices));

                    await publisher.PublishAsync(new BasketChangedEvent(userId), ct);

                    return Results.Ok(new { Success = true });
                })
                .AllowAnonymous()
                .WithName("UpsertBasket")
                .WithOpenApi();

            return app;
        }
    }
}
