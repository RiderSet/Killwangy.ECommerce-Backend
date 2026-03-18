using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
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

    private OutboxMessage(Guid id)
    {
        Id = id;
    }

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
        Attempts = 0;
        RetryCount = 0;
    }

    public OutboxMessage(
        Guid id,
        DateTime occurredOnUtc,
        string type,
        string payload)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Payload = payload;
    }

    // Cria mensagem a partir de DomainEvent
    public static OutboxMessage Create(IDomainEvent domainEvent)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        return new OutboxMessage(
            Guid.NewGuid(),
            domainEvent.GetType().Name,
            JsonSerializer.Serialize(domainEvent),
            domainEvent.OccurredOnUtc
        );
    }

    // Cria mensagem a partir de IntegrationEvent
    public static OutboxMessage CreateIntegrationEvent(IIntegrationEvent integrationEvent)
    {
        if (integrationEvent == null)
            throw new ArgumentNullException(nameof(integrationEvent));

        return new OutboxMessage(
            integrationEvent.Id,
            integrationEvent.OccurredOnUtc,
            integrationEvent.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(integrationEvent)
        );
    }

    // Marca como processada com sucesso
    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    // Marca como falha temporária
    public void MarkAsFailed(string message)
    {
        Attempts++;
        RetryCount++;
        Error = message;
    }

    public bool IsProcessed => ProcessedOnUtc.HasValue;

    // Lock para processamento concorrente
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

    // Marca falha e libera lock
    public void MarkFailed(string message)
    {
        Attempts++;
        RetryCount++;
        Error = message;
        Unlock();
    }

    internal static IIntegrationEvent CreateIntegrationEvent(PagamentoAprovadoIntegrationEvent integrationEvent)
    {
        throw new NotImplementedException();
    }
}