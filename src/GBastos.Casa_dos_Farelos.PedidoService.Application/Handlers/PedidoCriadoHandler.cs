using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pedidos;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.PedidoService.Application.Handlers;

public sealed class PedidoCriadoHandler : IIntegrationEventHandler<PedidoCriadoIntegrationEvent>
{
    public Task Handle(PedidoCriadoIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Baixando estoque do pedido {integrationEvent.PedidoId} do cliente {integrationEvent.ClienteId}, valor: {integrationEvent.Valor}");

        return Task.CompletedTask;
    }

    public Task Handle(GenericIntegrationEvent @event, CancellationToken cancellationToken)
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        if (string.IsNullOrWhiteSpace(@event.Payload))
            throw new InvalidOperationException("Payload vazio.");

        var integrationEvent = JsonSerializer.Deserialize<PedidoCriadoIntegrationEvent>(
            @event.Payload,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (integrationEvent is null)
            throw new InvalidOperationException("Evento inválido.");

        return Handle(integrationEvent, cancellationToken);
    }
}