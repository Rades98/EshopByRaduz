using Basket.Api.Dtos;
using Basket.Api.Events;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Endpoints
{
    internal static class UpdateBasketEndpoint
    {
        internal static IEndpointRouteBuilder MapUpdateBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/users/{userId:guid}/basket/{basketId:guid}",
                async (
                    [FromRoute] Guid userId,
                    [FromRoute] Guid basketId,
                    [FromBody] BasketDto req,
                    [FromServices] IConnectionMultiplexer redis,
                    [FromServices] KafkaEventPublisher publisher,
                    [FromServices] StockGrpcService stockGrpcService,
                    CancellationToken ct) =>
                {
                    var db = redis.GetDatabase();
                    var basketKey = $"basket:{userId}:{basketId}";

                    var insufficientItems = await req.GetInvalidStockItems(stockGrpcService, ct);

                    if (insufficientItems.Count != 0)
                    {
                        return Results.UnprocessableEntity(new
                        {
                            Error = "INSUFFICIENT_STOCK",
                            Items = insufficientItems
                        });
                    }

                    await db.StringSetAsync(basketKey, JsonSerializer.Serialize(req.Items));

                    await publisher.PublishAsync(new BasketChangedEvent(userId, basketId), ct);

                    return Results.Ok(new { Success = true });
                })
                .AllowAnonymous()
                .WithName("UpsertBasket")
                .WithOpenApi();

            return app;
        }
    }
}
