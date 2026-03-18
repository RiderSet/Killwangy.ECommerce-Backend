using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Compras;

public sealed class CompraCriadaIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public int Version { get; init; } = 1;

    public Guid CompraId { get; init; }
    public Guid ClienteId { get; init; }
    public decimal ValorTotal { get; init; }

    public IReadOnlyCollection<ItemCompraDto> Itens { get; init; } = [];

    public string EventType => throw new NotImplementedException();
}