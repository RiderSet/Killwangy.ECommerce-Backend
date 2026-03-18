using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.PedidoService.Api.Events;

public sealed class PedidoCriadoEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType => nameof(PedidoCriadoEvent);
    public int Version => 1;

    public Guid PedidoId { get; init; }
    public decimal Valor { get; init; }
}