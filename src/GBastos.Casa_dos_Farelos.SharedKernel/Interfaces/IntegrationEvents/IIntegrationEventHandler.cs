using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task Handle(GenericIntegrationEvent @event, CancellationToken ct);
    Task Handle<T>(T message, CancellationToken cancellationToken) where T : class, IIntegrationEvent;
    Task HandleAsync(TEvent integrationEvent, CancellationToken cancellationToken);
}