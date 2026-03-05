using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime? ProcessedOnUtc { get; private set; }
    public int Attempts { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }
    public Guid? LockId { get; private set; }
    public DateTime? LockedUntilUtc { get; private set; }

    private OutboxMessage() { }

    private OutboxMessage(
        Guid id,
        string type,
        string payload,
        DateTime occurredOnUtc)
    {
        Id = id;
        Type = type;
        Payload = payload;
        OccurredOnUtc = occurredOnUtc;
    }

    public static OutboxMessage Create(IDomainEvent domainEvent)
    {
        return new OutboxMessage(
            Guid.NewGuid(),
            domainEvent.GetType().Name,
            JsonSerializer.Serialize(domainEvent),
            domainEvent.OccurredOnUtc
        );
    }

    public static OutboxMessage CreateIntegrationEvent(object integrationEvent)
    {
        return new OutboxMessage(
            Guid.NewGuid(),
            integrationEvent.GetType().Name,
            JsonSerializer.Serialize(integrationEvent),
            DateTime.UtcNow
        );
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string message)
    {
        Attempts++;
        RetryCount++;
        Error = message;
    }

    public bool IsProcessed => ProcessedOnUtc.HasValue;

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

    public void MarkFailed(string message)
    {
        Attempts++;
        RetryCount++;
        Error = message;
        Unlock();
    }
}