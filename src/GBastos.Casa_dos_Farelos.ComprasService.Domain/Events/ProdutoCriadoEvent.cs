using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public sealed record ProdutoCriadoEvent(
    Guid ProdutoId,
    string Nome,
    decimal PrecoVenda
) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => ProdutoId;
    public string EventType => nameof(ProdutoCriadoEvent);
}