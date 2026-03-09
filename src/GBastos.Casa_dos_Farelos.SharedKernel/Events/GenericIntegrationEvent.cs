using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Events;

public sealed class GenericIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = default!;
    public string Payload { get; init; } = default!;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public DateTime OccurredOnUtc => OccurredOn;

    public int Version => 1;
}