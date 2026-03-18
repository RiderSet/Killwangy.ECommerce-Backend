using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(object integrationEvent, CancellationToken cancellationToken);
    Task<List<OutboxMessageDto>> GetPendingAsync(int take, CancellationToken ct);
    Task<bool> TryLockAsync(Guid id, DateTime lockUntilUtc, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<IEnumerable<IIntegrationEvent>> GetPendingAsync(CancellationToken cancellationToken);
}