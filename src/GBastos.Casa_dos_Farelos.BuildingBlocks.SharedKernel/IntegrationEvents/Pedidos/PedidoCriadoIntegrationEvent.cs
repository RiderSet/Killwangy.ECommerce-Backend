using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pedidos;

public sealed record PedidoCriadoIntegrationEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Valor
) : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType { get; init; } =
        nameof(PedidoCriadoIntegrationEvent);

    public int Version { get; init; } = 1;
}