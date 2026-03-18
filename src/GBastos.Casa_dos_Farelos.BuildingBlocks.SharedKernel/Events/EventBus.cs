using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed class EventBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly IMessageBroker _broker;

    public EventBus(
        IMediator mediator,
        IMessageBroker broker)
    {
        _mediator = mediator;
        _broker = broker;
    }

    public async Task Publish<T>(T @event, CancellationToken ct = default)
        where T : class
    {
        if (@event is IDomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent, ct);
            return;
        }

        if (@event is IIntegrationEvent integrationEvent)
        {
            await PublishAsync(integrationEvent, ct);
            return;
        }

        throw new InvalidOperationException(
            $"Tipo de evento não suportado: {@event.GetType().Name}");
    }

    public async Task Publish(object @event, CancellationToken ct = default)
    {
        switch (@event)
        {
            case IDomainEvent domainEvent:
                await _mediator.Publish(domainEvent, ct);
                break;

            case IIntegrationEvent integrationEvent:
                await PublishAsync(integrationEvent, ct);
                break;

            default:
                throw new InvalidOperationException(
                    $"Tipo de evento não suportado: {@event.GetType().Name}");
        }
    }

    public async Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken ct = default)
    {
        var topic = integrationEvent.GetType().Name;

        await _broker.PublishAsync(
            topic,
            integrationEvent,
            ct);
    }

    async Task IEventBus.PublishAsync<T>(T @event, CancellationToken ct)
    {
        await Publish(@event!, ct);
    }

    void IEventBus.Subscribe<TEvent, THandler>()
    {
        // Quando se usa MassTransit ou MediatR com DI,
        // as subscriptions são resolvidas automaticamente
        // pelos handlers registrados no container.
        // Portanto não é necessário implementar lógica aqui.
    }
}