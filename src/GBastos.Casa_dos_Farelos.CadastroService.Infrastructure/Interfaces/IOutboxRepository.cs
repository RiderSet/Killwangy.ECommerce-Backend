using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

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