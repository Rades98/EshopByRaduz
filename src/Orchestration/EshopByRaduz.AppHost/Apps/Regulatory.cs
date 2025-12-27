using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Regulatory
    {
        public static IResourceBuilder<ProjectResource> MapRegulatory
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var regulatoryDatabase = sql.AddDatabase("RegulatoryDatabase");

            var regulatory_grpc = builder.AddProject<Projects.Regulatory_Grpc>("regulatory-grpc")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(regulatoryDatabase)
                    .WaitFor(regulatoryDatabase)
                .WithReference(kafka);

            var worker = builder.AddProject<Projects.Regulatory_Worker>("regulatory-worker")
                .WithReference(regulatoryDatabase)
                    .WaitFor(regulatoryDatabase)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Regulatory-SupportingDomain"))
                .WithChildRelationship(regulatoryDatabase)
                .WithChildRelationship(regulatory_grpc)
                .WithChildRelationship(worker);

            return regulatory_grpc;
        }
    }
}
