using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public string? Type { get; private set; }
    public string? Payload { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }
    public string? EventType { get; set; }

    private OutboxMessage() { }

    public static OutboxMessage From(IIntegrationEvent @event, string payload)
    {
        return new OutboxMessage
        {
            Id = @event.Id,
            Type = @event.EventType,
            Payload = payload,
            OccurredOnUtc = @event.OccurredOnUtc
        };
    }

    public void MarkAsProcessed()
        => ProcessedOnUtc = DateTime.UtcNow;

    public void MarkAsFailed(string error)
        => Error = error;
}