using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MonitoringService.Config
{
    public static class RabbitMqConfig
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionFactory>(_ =>
                new ConnectionFactory
                {
                    HostName = config["RabbitMQ:Host"],
                    UserName = config["RabbitMQ:Username"],
                    Password = config["RabbitMQ:Password"],
                    DispatchConsumersAsync = true
                });

            return services;
        }
    }
}
