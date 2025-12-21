using Basket.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Endpoints
{
    internal static class GetBasketEndpoint
    {
        internal static IEndpointRouteBuilder MapGetBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/users/{userId:guid}/basket/{basketId:guid}",
                async (
                    [FromRoute] Guid userId,
                    [FromRoute] Guid basketId,
                    [FromServices] IConnectionMultiplexer redis,
                    CancellationToken ct) =>
                {
                    var db = redis.GetDatabase();
                    var basketKey = $"basket:{userId}:{basketId}";

                    var data = await db.StringGetAsync(basketKey);

                    if (!data.HasValue)
                    {
                        return Results.NotFound();
                    }

                    var items = JsonSerializer.Deserialize<List<BasketItemDto>>(data!) ?? [];

                    return Results.Ok(new BasketDto(items));
                })
                .AllowAnonymous()
                .WithName("GetBasket")
                .WithOpenApi();

            return app;
        }
    }
}
