namespace GBastos.Casa_dos_Farelos.Messaging.Abstractions;

public abstract class BaseIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

    public string EventType => GetType().Name;

    public abstract int Version { get; }
}