using Basket.Api.Dtos;
using Basket.Api.Events;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Endpoints
{
    internal static class CreateBasketEndpoint
    {
        public static IEndpointRouteBuilder MapCreateBasketEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/users/{userId:guid}/basket",
                async (
                    [FromRoute] Guid userId,
                    [FromBody] BasketDto req,
                    [FromServices] IConnectionMultiplexer redis,
                    [FromServices] KafkaEventPublisher publisher,
                    [FromServices] StockGrpcService stockGrpcService,
                    CancellationToken ct) =>
                {
                    var basketId = Guid.NewGuid();
                    var db = redis.GetDatabase();

                    var insufficientItems = await req.GetInvalidStockItems(stockGrpcService, ct);

                    if (insufficientItems.Count != 0)
                    {
                        return Results.UnprocessableEntity(new
                        {
                            Error = "INSUFFICIENT_STOCK",
                            Items = insufficientItems
                        });
                    }

                    var basketKey = $"basket:{userId}:{basketId}";
                    await db.StringSetAsync(basketKey, JsonSerializer.Serialize(req.Items));

                    await publisher.PublishAsync(new BasketChangedEvent(userId, basketId), ct);

                    return Results.Created(
                        $"/users/{userId}/basket/{basketId}",
                        new
                        {
                            Success = true,
                            BasketId = basketId
                        });
                })
                .AllowAnonymous()
                .WithName("CreateBasket")
                .WithOpenApi();

            return endpoints;
        }
    }
}