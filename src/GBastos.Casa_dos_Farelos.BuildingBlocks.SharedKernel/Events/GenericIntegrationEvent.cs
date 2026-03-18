using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed class GenericIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = default!;
    public string Payload { get; init; } = default!;
    public string TenantId { get; init; } = default!;
    public string Source { get; init; } = default!;

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public DateTime OccurredOnUtc => OccurredOn;

    public int Version => 1;
}