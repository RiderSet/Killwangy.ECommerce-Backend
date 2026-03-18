namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(
        BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos.PagamentoAprovadoIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);

    Task<List<OutboxMessage>> GetPendingAsync(
        int batchSize,
        CancellationToken cancellationToken = default);

    Task LockAsync(
        OutboxMessage message,
        Guid lockId,
        TimeSpan duration,
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        OutboxMessage message,
        string error,
        CancellationToken cancellationToken = default);
}