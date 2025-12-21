namespace EshopByRaduz.AppHost.Domains
{
    internal static class Order
    {
        public static IResourceBuilder<ProjectResource> MapOrder
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var orderDatabase = sql.AddDatabase("OrderDatabase");

            var order = builder.AddProject<Projects.Order_Api>("orderapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(orderDatabase)
                    .WaitFor(orderDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            return order;
        }
    }
}
