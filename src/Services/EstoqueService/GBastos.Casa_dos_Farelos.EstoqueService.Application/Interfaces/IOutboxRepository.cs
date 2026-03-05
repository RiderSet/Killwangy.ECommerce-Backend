using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(IDomainEvent domainEvent, CancellationToken ct);

    Task AddAsync<T>(T integrationEvent, CancellationToken ct)
        where T : IIntegrationEvent;

    Task<List<OutboxMessageDto>> GetUnprocessedAsync(
        int take,
        CancellationToken ct);

    Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct);
    Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken ct);
    Task<bool> TryLockAsync(Guid messageId, DateTime lockUntilUtc, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}