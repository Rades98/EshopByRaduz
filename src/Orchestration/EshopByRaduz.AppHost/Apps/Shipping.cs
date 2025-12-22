using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Shipping
    {
        public static (IResourceBuilder<ProjectResource> Shipping, IResourceBuilder<ProjectResource> ShippingGrpc) MapShipping
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var shippingDatabase = sql.AddDatabase("ShippingDatabase");

            var shippingGrpc = builder.AddProject<Projects.Shipping_Grpc>("shipping-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(shippingDatabase)
                    .WaitFor(shippingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var shipping = builder.AddProject<Projects.Shipping_Api>("shipping-api")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(shippingDatabase)
                    .WaitFor(shippingDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Shipping-CoreDomain"))
                .WithChildRelationship(shippingDatabase)
                .WithChildRelationship(shippingGrpc)
                .WithChildRelationship(shipping);

            return new(shipping, shippingGrpc);
        }
    }
}
