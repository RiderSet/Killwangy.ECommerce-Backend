using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Events;

public sealed record UsuarioCriadoDomainEvent(
    Guid UsuarioId,
    string Nome,
    string Email
) : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
