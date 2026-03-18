using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueConfirmadoEvent(
    Guid ProdutoId,
    int Quantidade
) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => ProdutoId;
    public string EventType => nameof(EstoqueConfirmadoEvent);
}