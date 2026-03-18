using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Events;

public sealed class ClienteCriadoEvent : IDomainEvent
{
    public Guid ClienteId { get; }
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    public Guid AggregateId => ClienteId;
    public string EventType => GetType().Name;

    public ClienteCriadoEvent(Guid clienteId)
    {
        if (clienteId == Guid.Empty)
            throw new ArgumentException("ClienteId inválido.", nameof(clienteId));
        ClienteId = clienteId;
    }
}