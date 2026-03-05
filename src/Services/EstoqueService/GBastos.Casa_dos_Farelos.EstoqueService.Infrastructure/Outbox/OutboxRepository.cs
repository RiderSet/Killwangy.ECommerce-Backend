using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
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
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == messageId, ct);

        if (message == null)
            return;

        message.MarkAsProcessed();
    }

    public async Task MarkAsFailedAsync(
        Guid messageId,
        string error,
        CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == messageId, ct);

        if (message == null)
            return;

        message.MarkFailed(error);
    }

    public async Task<bool> TryLockAsync(
        Guid messageId,
        DateTime lockUntilUtc,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.OutboxMessages
            .Where(x =>
                x.Id == messageId &&
                (x.LockedUntilUtc == null || x.LockedUntilUtc < now))
            .ExecuteUpdateAsync(x =>
                x.SetProperty(p => p.LockedUntilUtc, lockUntilUtc),
                ct);

        return affected > 0;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}