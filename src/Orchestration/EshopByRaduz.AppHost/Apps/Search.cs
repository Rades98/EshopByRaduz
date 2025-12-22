using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Search
    {
        public static IResourceBuilder<ProjectResource> MapSearch
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka)
        {
            var elasticsearch = builder.AddElasticsearch("elasticsearch");

            var search = builder.AddProject<Projects.Search_Api>("search-api");

            var searchConsumer = builder.AddProject<Projects.Search_Consumer>("search-consumer")
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Search-CoreDomain"))
                .WithChildRelationship(elasticsearch)
                .WithChildRelationship(search)
                .WithChildRelationship(searchConsumer);

            return search;
        }
    }
}
