using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;

public class OutboxMessage : BaseEntity
{
    private string? error;

    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public DateTime? LockedUntilUtc { get; private set; }
    public Guid? LockId { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(object @event)
    {
        Id = Guid.NewGuid();

        Type = @event.GetType().Name;

        Payload = JsonSerializer.Serialize(
            @event,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

        OccurredOnUtc = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
        Unlock();
    }

    public void MarkAsFailed(string error)
    {
        RetryCount++;
        Error = error;

        Unlock();
    }

    public void Lock(Guid lockId, TimeSpan duration)
    {
        LockId = lockId;
        LockedUntilUtc = DateTime.UtcNow.Add(duration);
    }

    public void Unlock()
    {
        LockId = null;
        LockedUntilUtc = null;
    }

    internal void MarkFailed(string message)
    {
        RetryCount++;
        Error = error;
        Unlock();
    }

    public bool IsProcessingLocked =>
        LockedUntilUtc.HasValue &&
        LockedUntilUtc > DateTime.UtcNow;

    public bool IsProcessed => ProcessedOnUtc.HasValue;
}