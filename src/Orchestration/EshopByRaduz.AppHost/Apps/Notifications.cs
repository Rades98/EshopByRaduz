using EshopByRaduz.AppHost.Resources;

namespace EshopByRaduz.AppHost.Apps
{
    internal static class Notifications
    {
        public static IResourceBuilder<ProjectResource> MapNotifications
            (this IDistributedApplicationBuilder builder, IResourceBuilder<KafkaServerResource> kafka)
        {
            var notifications = builder.AddProject<Projects.Notifications_Service>("notifications-service")
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
                .WithReference(kafka)
                    .WaitFor(kafka);

            var group = builder.AddResource(new GroupResource("Notifications-Channel"))
                .WithChildRelationship(notifications);

            return notifications;
        }
    }
}
