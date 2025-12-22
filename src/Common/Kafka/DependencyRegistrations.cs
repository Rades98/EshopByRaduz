using Microsoft.Extensions.DependencyInjection;

namespace Kafka
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddKafkaPublisher(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaPublisher, KafkaEventPublisher>();

            return services;
        }
    }
}
