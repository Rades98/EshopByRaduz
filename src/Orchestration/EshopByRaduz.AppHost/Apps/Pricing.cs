using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Pricing
    {
        public static IResourceBuilder<ProjectResource> MapPricing
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var pricingDatabase = sql.AddDatabase("PricingDatabase");

            var pricingSeed = builder.AddProject<Projects.Pricing_Api>("pricing-api-seed")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithArgs("--seed")
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var pricingGrpc = builder.AddProject<Projects.Pricing_Grpc>("pricing-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                .WaitForCompletion(pricingSeed);

            var pricingConsumer = builder.AddProject<Projects.Pricing_Consumer>("pricing-consumer")
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WaitForCompletion(pricingSeed);

            var group = builder.AddResource(new GroupResource("Pricing-CoreDomain"))
                .WithChildRelationship(pricingDatabase)
                .WithChildRelationship(pricingGrpc)
                .WithChildRelationship(pricingSeed)
                .WithChildRelationship(pricingConsumer);

            return pricingGrpc;
        }
    }
}
