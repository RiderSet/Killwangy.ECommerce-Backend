namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken ct);

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

    Task LockAsync(
        Guid id,
        Guid lockId,
        TimeSpan duration,
        CancellationToken ct);
}