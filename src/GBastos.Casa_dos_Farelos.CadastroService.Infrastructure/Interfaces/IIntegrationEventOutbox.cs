using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(
        PagamentoAprovadoIntegrationEvent integrationEvent,
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