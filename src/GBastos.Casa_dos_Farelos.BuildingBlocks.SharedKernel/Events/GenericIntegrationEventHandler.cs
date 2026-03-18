using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed class GenericIntegrationEventHandler
    : IIntegrationEventHandler<GenericIntegrationEvent>
{
    public Task Handle(GenericIntegrationEvent @event, CancellationToken ct)
    {
        Console.WriteLine("Evento genérico recebido:");
        Console.WriteLine($"Id: {@event.Id}");
        Console.WriteLine($"Tipo: {@event.EventType}");
        Console.WriteLine($"Payload: {@event.Payload}");
        Console.WriteLine($"Data: {@event.OccurredOnUtc}");

        return Task.CompletedTask;
    }
}