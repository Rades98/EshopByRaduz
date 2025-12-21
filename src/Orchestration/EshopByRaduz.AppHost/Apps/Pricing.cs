using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Pricing
    {
        public static IResourceBuilder<ProjectResource> MapPricing
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var pricingDatabase = sql.AddDatabase("PricingDatabase");

            var group = builder.AddResource(new GroupResource("Pricing-CoreDomain"));

            var pricingGrpc = builder.AddProject<Projects.Pricing_Grpc>("pricing-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithParentRelationship(group);

            var pricingSeed = builder.AddProject<Projects.Pricing_Api>("pricing-api-seed")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithArgs("--seed")
                .WithReference(pricingDatabase)
                    .WaitFor(pricingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithParentRelationship(group);

            return pricingGrpc;
        }
    }
}
