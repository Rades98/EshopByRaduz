using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Stock
    {
        public static (IResourceBuilder<ProjectResource> Stock, IResourceBuilder<ProjectResource> StockGrpc) MapStock
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var stockDatabase = sql.AddDatabase("StockDatabase");

            var stockSeed = builder.AddProject<Projects.Stock_Api>("stockapiseed")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithArgs("--seed")
                .WithHttpEndpoint(port: 5555, name: "stockSeed")
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase);

            var stock = builder.AddProject<Projects.Stock_Api>("stockapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .MapUrlsToScalar()
                .WaitForCompletion(stockSeed)
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var stockGrpc = builder.AddProject<Projects.Stock_Grpc>("stock-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WaitForCompletion(stockSeed)
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase);

            var group = builder.AddResource(new GroupResource("Stock-CoreDomain"))
                .WithChildRelationship(stockDatabase)
                .WithChildRelationship(stock)
                .WithChildRelationship(stockGrpc)
                .WithChildRelationship(stockSeed);

            return new(stock, stockGrpc);
        }
    }
}
