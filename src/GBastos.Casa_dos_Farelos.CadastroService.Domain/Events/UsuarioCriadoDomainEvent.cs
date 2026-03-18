using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Events;

public sealed record UsuarioCriadoDomainEvent(
    Guid UsuarioId,
    string Nome,
    string Email
) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => UsuarioId;
    public string EventType => nameof(UsuarioCriadoDomainEvent);
}