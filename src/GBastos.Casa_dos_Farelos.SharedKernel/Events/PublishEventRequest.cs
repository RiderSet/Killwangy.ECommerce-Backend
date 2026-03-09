namespace GBastos.Casa_dos_Farelos.SharedKernel.Events;

public sealed class PublishEventRequest
{
    public string EventType { get; init; } = default!;
    public string Payload { get; init; } = default!;
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public int Version { get; init; } = 1;
}