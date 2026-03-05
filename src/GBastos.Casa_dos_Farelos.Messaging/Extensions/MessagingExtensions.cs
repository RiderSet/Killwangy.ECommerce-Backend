using GBastos.Casa_dos_Farelos.Messaging.Abstractions;
using GBastos.Casa_dos_Farelos.Messaging.Outbox;
using GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;
using GBastos.Casa_dos_Farelos.Messaging.Serialization;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddSingleton<IEventSerializer, SystemTextJsonEventSerializer>();
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}