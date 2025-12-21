namespace EshopByRaduz.AppHost.Domains
{
    internal static class Basket
    {
        public static (IResourceBuilder<ProjectResource> Basket, IResourceBuilder<ProjectResource> BasketGrpc) MapBasket(this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<ProjectResource> stockGrpc)
        {
            var redisDb = builder.AddRedis("RedisDb")
                .WithLifetime(ContainerLifetime.Persistent)
                .WithRedisInsight();

            var basket = builder.AddProject<Projects.Basket_Api>("basketapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .MapUrlsToScalar()
                .WithReference(redisDb)
                    .WaitFor(redisDb)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithReference(stockGrpc)
                    .WaitFor(stockGrpc);

            var basketGrpc = builder.AddProject<Projects.Basket_Grpc>("basket-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(redisDb)
                    .WaitFor(redisDb)
                .WithReference(kafka)
                    .WaitFor(kafka);

            return new(basket, basketGrpc);
        }
    }
}
