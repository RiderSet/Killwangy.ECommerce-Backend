using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.PagamentoService.Application.DTOs;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<OutboxMessageDto>> GetUnprocessedAsync(
        int take,
        CancellationToken cancellationToken = default);

    Task<bool> TryLockAsync(
        Guid messageId,
        DateTime lockUntilUtc,
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        Guid messageId,
        string error,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}