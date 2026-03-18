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
            var host = configuration["RabbitMQ:Host"]
                ?? throw new InvalidOperationException("RabbitMQ:Host não configurado");

            var user = configuration["RabbitMQ:User"]
                ?? throw new InvalidOperationException("RabbitMQ:User não configurado");

            var pass = configuration["RabbitMQ:Password"]
                ?? throw new InvalidOperationException("RabbitMQ:Password não configurado");

            var factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = pass
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IEventPublisher, RabbitMqPublisher>();

        return services;
    }
}