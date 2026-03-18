using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly EstoqueDbContext _context;

    public OutboxRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<T>(T integrationEvent, CancellationToken ct)
        where T : IIntegrationEvent
    {
        var message = OutboxMessage.CreateIntegrationEvent(integrationEvent);
        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task AddAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        var message = OutboxMessage.CreateDomainEvent(domainEvent);
        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task<List<OutboxMessageDto>> GetUnprocessedAsync(
        int take,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        return await _context.OutboxMessages
            .Where(x =>
                x.ProcessedOnUtc == null &&
                (x.LockedUntilUtc == null || x.LockedUntilUtc < now) &&
                (x.NextRetryAtUtc == null || x.NextRetryAtUtc < now))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .Select(x => new OutboxMessageDto
            {
                Id = x.Id,
                Type = x.Type,
                Payload = x.Payload,
                OccurredOnUtc = x.OccurredOnUtc
            })
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct)
    {
        await _context.OutboxMessages
            .Where(x => x.Id == messageId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.ProcessedOnUtc, DateTime.UtcNow)
                .SetProperty(p => p.Error, (string?)null),
                ct);
    }

    public async Task MarkAsFailedAsync(
        Guid messageId,
        string error,
        CancellationToken ct)
    {
        await _context.OutboxMessages
            .Where(x => x.Id == messageId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.Error, error)
                .SetProperty(p => p.Attempts, p => p.Attempts + 1)
                .SetProperty(p => p.NextRetryAtUtc,
                    DateTime.UtcNow.AddSeconds(2)), // ou exponencial
                ct);
    }

    public async Task<bool> TryLockAsync(
        Guid messageId,
        DateTime lockUntilUtc,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var lockId = Guid.NewGuid();

        var affected = await _context.OutboxMessages
            .Where(x =>
                x.Id == messageId &&
                (x.LockedUntilUtc == null || x.LockedUntilUtc < now))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.LockedUntilUtc, lockUntilUtc)
                .SetProperty(p => p.LockId, lockId),
                ct);

        return affected > 0;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);

    public Task AddAsync(object integrationEvent, CancellationToken cancellationToken)
    {
        if (integrationEvent is not IIntegrationEvent @event)
            throw new ArgumentException("Object must implement IIntegrationEvent");

        return AddAsync(@event, cancellationToken);
    }

    public Task<List<OutboxMessageDto>> GetPendingAsync(int take, CancellationToken cancellationToken)
    {
        return GetUnprocessedAsync(take, cancellationToken);
    }

    public Task<IEnumerable<IIntegrationEvent>> GetPendingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}