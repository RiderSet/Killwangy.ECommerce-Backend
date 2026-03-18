using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using MassTransit;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Publish<T>(T @event, CancellationToken ct = default)
        where T : class
    {
        return _publishEndpoint.Publish(@event, ct);
    }

    public Task Publish(object @event, CancellationToken ct = default)
    {
        return _publishEndpoint.Publish(@event, ct);
    }

    public Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken ct = default)
        where TEvent : class, IIntegrationEvent
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        return _publishEndpoint.Publish(integrationEvent, ct);
    }

    public void Subscribe<TEvent, THandler>()
        where TEvent : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        // MassTransit faz subscription via DI + AddConsumers
        // Nada precisa ser feito aqui
    }
}