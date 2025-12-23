using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Pricing
    {
        public static (IResourceBuilder<ProjectResource> PricingGrpc, IResourceBuilder<ProjectResource> PricingApi) MapPricing
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var pricingDatabase = sql.AddDatabase("PricingDatabase");

            var pricingConsumer = builder.AddProject<Projects.Pricing_Worker>("pricing-worker")
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var pricingApi = builder.AddProject<Projects.Pricing_Api>("pricing-api")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .MapUrlsToScalar()
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var pricingGrpc = builder.AddProject<Projects.Pricing_Grpc>("pricing-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka);



            var group = builder.AddResource(new GroupResource("Pricing-CoreDomain"))
                .WithChildRelationship(pricingDatabase)
                .WithChildRelationship(pricingApi)
                .WithChildRelationship(pricingGrpc)
                .WithChildRelationship(pricingConsumer);

            return new(pricingGrpc, pricingApi);
        }
    }
}
