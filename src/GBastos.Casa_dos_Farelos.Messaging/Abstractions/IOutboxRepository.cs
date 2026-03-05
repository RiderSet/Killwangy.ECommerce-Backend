using GBastos.Casa_dos_Farelos.Messaging.Outbox;

namespace GBastos.Casa_dos_Farelos.Messaging.Abstractions;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetPendingAsync(
        int take,
        CancellationToken ct);

    Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken ct);

    Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken ct);
}