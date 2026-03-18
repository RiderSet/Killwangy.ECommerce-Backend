using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pedidos;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public sealed class PedidoCriadoHandler
    : IIntegrationEventHandler<PedidoCriadoIntegrationEvent>
{
    public Task Handle(GenericIntegrationEvent @event, CancellationToken ct)
    {
        Console.WriteLine($"Evento genérico recebido: {@event.EventType}");
        return Task.CompletedTask;
    }

    public Task Handle<T>(T message, CancellationToken cancellationToken)
        where T : class, IIntegrationEvent
    {
        Console.WriteLine($"Evento tipado recebido: {typeof(T).Name}");
        return Task.CompletedTask;
    }

    public Task Handle(PedidoCriadoIntegrationEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(
        PedidoCriadoIntegrationEvent @event,
        CancellationToken ct)
    {
        Console.WriteLine($"Baixando estoque do pedido {@event.PedidoId}");
        return Task.CompletedTask;
    }
}