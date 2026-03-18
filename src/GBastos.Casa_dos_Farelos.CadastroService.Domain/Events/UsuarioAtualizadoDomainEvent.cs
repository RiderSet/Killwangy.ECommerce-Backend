using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Events;

public sealed record UsuarioAtualizadoDomainEvent(
    Guid UsuarioId,
    string Nome,
    string Email
) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public Guid EventId => Guid.NewGuid();

    public Guid AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();
}