using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pedidos;

namespace GBastos.Casa_dos_Farelos.PedidoService.Application.Handlers;

public sealed class PedidoCriadoHandler
    : IIntegrationEventHandler<PedidoCriadoIntegrationEvent>
{
    public Task HandleAsync(
        PedidoCriadoIntegrationEvent integrationEvent,
        CancellationToken ct)
    {
        Console.WriteLine($"Baixando estoque do pedido {integrationEvent.PedidoId}");

        // Chamadas para:
        // - EstoqueService
        // - Repository
        // - Outros eventos

        return Task.CompletedTask;
    }

    public Task Handle<T>(
        T message,
        CancellationToken cancellationToken)
        where T : class, IIntegrationEvent
    {
        return HandleAsync(
            message as PedidoCriadoIntegrationEvent
                ?? throw new InvalidCastException(),
            cancellationToken);
    }
}