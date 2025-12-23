using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Checkout
    {
        public static IResourceBuilder<ProjectResource> MapCheckout(
            this IDistributedApplicationBuilder builder,
            IResourceBuilder<KafkaServerResource> kafka,
            IResourceBuilder<SqlServerServerResource> sql,
            IResourceBuilder<ProjectResource> stockGrpc,
            IResourceBuilder<ProjectResource> pricingGrpc,
            IResourceBuilder<ProjectResource> shippingGrpc,
            IResourceBuilder<ProjectResource> regulatoryGrpc
            )
        {
            var checkoutDatabase = sql.AddDatabase("CheckoutDatabase");

            var group = builder.AddResource(new GroupResource("Checkout-CoreDomain-Orchestrator"));

            var checkout = builder.AddProject<Projects.Checkout_Api>("checkoutapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(checkoutDatabase)
                    .WaitFor(checkoutDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithReference(stockGrpc)
                    .WaitFor(stockGrpc)
                .WithReference(pricingGrpc)
                    .WaitFor(pricingGrpc)
                .WithReference(shippingGrpc)
                    .WaitFor(shippingGrpc)
                .WithParentRelationship(group)
                    .WaitFor(regulatoryGrpc)
                .WithParentRelationship(regulatoryGrpc);

            return checkout;
        }
    }
}
