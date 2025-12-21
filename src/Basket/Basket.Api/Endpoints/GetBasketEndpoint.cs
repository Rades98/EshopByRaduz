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
            app.MapGet("/users/{userId:guid}/basket/",
                async (
                    [FromRoute] Guid userId,
                    [FromServices] IConnectionMultiplexer redis,
                    CancellationToken ct) =>
                {
                    var db = redis.GetDatabase();
                    var basketKey = $"basket:{userId}";

                    var data = await db.StringGetAsync(basketKey);

                    if (!data.HasValue)
                    {
                        return Results.NotFound();
                    }

                    var basket = JsonSerializer.Deserialize<BasketDto>(data!);

                    return Results.Ok(basket);
                })
                .AllowAnonymous()
                .WithName("GetBasket")
                .WithOpenApi();

            return app;
        }
    }
}
