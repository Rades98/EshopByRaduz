using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Catalog
    {
        public static IResourceBuilder<ProjectResource> MapCatalog
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka, IResourceBuilder<SqlServerServerResource> sql)
        {
            var redisCahe = builder.AddRedis("RedisCache")
                .WithLifetime(ContainerLifetime.Persistent)
                .WithRedisInsight();

            var catalogDatabase = sql.AddDatabase("CatalogDatabase");

            var catalog = builder.AddProject<Projects.Catalog_Api>("catalogapi")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(catalogDatabase)
                    .WaitFor(catalogDatabase)
                .WithReference(redisCahe)
                    .WaitFor(redisCahe)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Catalog-SupportingDomain"))
                .WithChildRelationship(catalogDatabase)
                .WithChildRelationship(catalog)
                .WithChildRelationship(redisCahe);

            return catalog;
        }
    }
}
