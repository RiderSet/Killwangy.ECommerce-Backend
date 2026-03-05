using MediatR;

namespace GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.General;

public abstract record IntegrationEvent : INotification
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().Name;
    public int Version { get; init; } = 1;
}