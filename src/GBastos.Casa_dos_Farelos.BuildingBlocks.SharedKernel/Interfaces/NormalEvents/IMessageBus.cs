using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

public interface IMessageBus
{
    Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken ct = default)
        where TEvent : class, IIntegrationEvent;

    Task SendAsync<TCommand>(
        TCommand command,
        CancellationToken ct = default)
        where TCommand : class;

    void Subscribe<TEvent, THandler>()
        where TEvent : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>;
}