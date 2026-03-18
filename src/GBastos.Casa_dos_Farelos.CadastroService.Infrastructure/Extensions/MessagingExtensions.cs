using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Messaging.EventBus;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Messaging.Serialization;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Extensions;

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