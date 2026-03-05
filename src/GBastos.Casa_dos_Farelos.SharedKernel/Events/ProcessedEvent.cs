namespace GBastos.Casa_dos_Farelos.Messaging.Events;

public sealed class ProcessedEvent
{
    public Guid EventId { get; private set; }
    public DateTime ProcessedOnUtc { get; private set; }
}
