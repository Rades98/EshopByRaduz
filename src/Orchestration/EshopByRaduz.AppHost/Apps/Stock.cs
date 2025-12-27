using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Stock
    {
        public static (IResourceBuilder<ProjectResource> Stock, IResourceBuilder<ProjectResource> StockGrpc) MapStock
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var stockDatabase = sql.AddDatabase("StockDatabase");

            var worker = builder.AddProject<Projects.Stock_Worker>("stock-worker")
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var stock = builder.AddProject<Projects.Stock_Api>("stock-api")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .MapUrlsToScalar()
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var stockGrpc = builder.AddProject<Projects.Stock_Grpc>("stock-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(stockDatabase)
                    .WaitFor(stockDatabase);

            var group = builder.AddResource(new GroupResource("Stock-SupportingDomain"))
                .WithChildRelationship(stockDatabase)
                .WithChildRelationship(worker)
                .WithChildRelationship(stock)
                .WithChildRelationship(stockGrpc);

            return new(stock, stockGrpc);
        }
    }
}
