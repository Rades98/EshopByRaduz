using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Shipping
    {
        public static (IResourceBuilder<ProjectResource> Shipping, IResourceBuilder<ProjectResource> ShippingGrpc) MapShipping
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var shippingDatabase = sql.AddDatabase("ShippingDatabase");

            var group = builder.AddResource(new GroupResource("Shipping-CoreDomain"));

            var shippingGrpc = builder.AddProject<Projects.Shipping_Grpc>("shipping-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(shippingDatabase)
                    .WaitFor(shippingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithParentRelationship(group);

            var shipping = builder.AddProject<Projects.Shipping_Api>("shipping-api")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(shippingDatabase)
                    .WaitFor(shippingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithParentRelationship(group);

            return new(shipping, shippingGrpc);
        }
    }
}
