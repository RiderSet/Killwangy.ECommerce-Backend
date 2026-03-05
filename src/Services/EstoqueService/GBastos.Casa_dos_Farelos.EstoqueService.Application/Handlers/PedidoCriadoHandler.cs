using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pedidos;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public sealed class PedidoCriadoHandler
    : IIntegrationEventHandler<PedidoCriadoIntegrationEvent>
{
    public Task HandleAsync(
        PedidoCriadoIntegrationEvent @event,
        CancellationToken ct)
    {
        Console.WriteLine($"Baixando estoque do pedido {@event.PedidoId}");
        return Task.CompletedTask;
    }
}