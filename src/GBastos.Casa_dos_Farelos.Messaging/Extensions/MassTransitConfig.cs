using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.Extensions;

public static class MassTransitConfig
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration config)
    {
        var host = config["RabbitMQ:Host"]
            ?? throw new InvalidOperationException("RabbitMQ Host não configurado");

        var user = config["RabbitMQ:User"]
            ?? throw new InvalidOperationException("RabbitMQ User não configurado");

        var password = config["RabbitMQ:Password"]
            ?? throw new InvalidOperationException("RabbitMQ Password não configurado");

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(host, "/", h =>
                {
                    h.Username(user);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}