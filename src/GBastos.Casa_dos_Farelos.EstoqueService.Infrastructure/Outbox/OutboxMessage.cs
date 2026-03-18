using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Common;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public int Version { get; private set; }

    public string? IdempotencyKey { get; private set; }
    public DateTime? NextRetryAtUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public int Attempts { get; private set; }

    public string? Error { get; private set; }

    public Guid? LockId { get; private set; }
    public DateTime? LockedUntilUtc { get; private set; }

    private OutboxMessage() { }

    private OutboxMessage(
        Guid id,
        string type,
        string payload,
        DateTime occurredOnUtc,
        string? idempotencyKey,
        int version)
    {
        Id = id;
        Type = type;
        Payload = payload;
        OccurredOnUtc = occurredOnUtc;
        IdempotencyKey = idempotencyKey;
        Version = version;
    }

    public static OutboxMessage CreateDomainEvent(IDomainEvent e)
        => new(
            Guid.NewGuid(),
            e.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(e),
            e.OccurredOnUtc,
            e.Id.ToString(),
            1);

    public static OutboxMessage CreateIntegrationEvent(IIntegrationEvent e)
        => new(
            Guid.NewGuid(),
            e.EventType,
            JsonSerializer.Serialize(e),
            e.OccurredOnUtc,
            e.Id.ToString(),
            e.Version);

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
        Attempts++;
        Unlock();
    }

    public void MarkFailed(string error)
    {
        Attempts++;

        Error = error;

        var delay = TimeSpan.FromSeconds(Math.Pow(2, Attempts));

        NextRetryAtUtc = DateTime.UtcNow.Add(delay);

        Unlock();
    }

    public bool IsDeadLetter => Attempts >= 5;

    public void Lock(DateTime lockUntilUtc)
    {
        LockId = Guid.NewGuid();
        LockedUntilUtc = lockUntilUtc;
    }

    public void Unlock()
    {
        LockId = null;
        LockedUntilUtc = null;
    }
}