using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;

public abstract record IntegrationEventBase : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public abstract string EventType { get; }
    public int Version { get; init; } = 1;
}