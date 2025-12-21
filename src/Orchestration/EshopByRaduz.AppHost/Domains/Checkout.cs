namespace EshopByRaduz.AppHost.Domains
{
    internal static class Checkout
    {
        public static IResourceBuilder<ProjectResource> MapCheckout
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql, IResourceBuilder<ProjectResource> stockGrpc)
        {
            var checkoutDatabase = sql.AddDatabase("CheckoutDatabase");


            var checkout = builder.AddProject<Projects.Checkout_Api>("checkoutapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(checkoutDatabase)
                    .WaitFor(checkoutDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithReference(stockGrpc)
                    .WaitFor(stockGrpc);

            return checkout;
        }
    }
}
