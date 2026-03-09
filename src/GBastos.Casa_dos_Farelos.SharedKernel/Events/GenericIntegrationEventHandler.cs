using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Events;

public sealed class GenericIntegrationEventHandler
    : IIntegrationEventHandler<GenericIntegrationEvent>
{
    public Task Handle(GenericIntegrationEvent @event, CancellationToken ct)
    {
        return HandleAsync(@event, ct);
    }

    public Task HandleAsync(
        GenericIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("Evento genérico recebido:");
        Console.WriteLine($"Id: {integrationEvent.Id}");
        Console.WriteLine($"Tipo: {integrationEvent.EventType}");
        Console.WriteLine($"Payload: {integrationEvent.Payload}");
        Console.WriteLine($"Data: {integrationEvent.OccurredOnUtc}");

        return Task.CompletedTask;
    }

    Task IIntegrationEventHandler<GenericIntegrationEvent>.Handle<T>(T message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}