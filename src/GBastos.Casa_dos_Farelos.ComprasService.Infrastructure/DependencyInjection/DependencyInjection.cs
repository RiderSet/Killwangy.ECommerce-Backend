using GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.RabbitMQ;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"],
                UserName = configuration["RabbitMQ:User"],
                Password = configuration["RabbitMQ:Password"]
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IEventPublisher, RabbitMqPublisher>();

        return services;
    }
}