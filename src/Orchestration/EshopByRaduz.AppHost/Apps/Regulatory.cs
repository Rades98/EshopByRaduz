using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Regulatory
    {
        public static IResourceBuilder<ProjectResource> MapRegulatory
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var regulatoryDatabase = sql.AddDatabase("RegulatoryDatabase");

            var regulatory = builder.AddProject<Projects.Regulatory_Grpc>("regulatory-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(regulatoryDatabase)
                    .WaitFor(regulatoryDatabase)
                .WithReference(kafka);

            var regulatoryConsumer = builder.AddProject<Projects.Regulatory_Consumer>("regulatory-consumer")
                .WithReference(regulatoryDatabase)
                    .WaitFor(regulatoryDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Regulatory-CoreDomain"))
                .WithChildRelationship(regulatoryDatabase)
                .WithChildRelationship(regulatory)
                .WithChildRelationship(regulatoryConsumer);

            return regulatory;
        }
    }
}
