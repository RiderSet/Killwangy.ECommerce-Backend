using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

public record VeiculoCriadoEvent(
    Guid VeiculoId,
    string Placa) : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    public string EventType => nameof(VeiculoCriadoEvent);
    public int Version => 1;
}