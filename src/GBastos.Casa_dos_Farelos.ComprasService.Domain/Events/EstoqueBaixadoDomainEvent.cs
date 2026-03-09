using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public sealed record EstoqueBaixadoDomainEvent(
    Guid ProdutoId,
    string NomeProduto,
    int QuantidadeBaixada
) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => ProdutoId;
    public string EventType => nameof(EstoqueBaixadoDomainEvent);
}