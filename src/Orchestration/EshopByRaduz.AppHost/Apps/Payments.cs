using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Payments
    {
        public static IResourceBuilder<ProjectResource> MapPayments
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var paymentsDatabase = sql.AddDatabase("PaymentsDatabase");

            var group = builder.AddResource(new GroupResource("Payments-GenericSubdomain"));

            var payments = builder.AddProject<Projects.Payments_Grpc>("payments-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(paymentsDatabase)
                    .WaitFor(paymentsDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka)
                .WithParentRelationship(group);

            return payments;
        }
    }
}
