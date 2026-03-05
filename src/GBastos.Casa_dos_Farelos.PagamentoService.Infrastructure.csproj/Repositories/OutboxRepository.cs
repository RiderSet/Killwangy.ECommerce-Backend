using GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Repositories;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly PagamentoDbContext _context;

    public OutboxRepository(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        OutboxMessagePG message,
        CancellationToken ct)
    {
        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task<List<OutboxMessagePG>> GetPendingAsync(
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
            .ToListAsync(ct);
    }

    public async Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message == null)
            return;

        message.MarkAsProcessed();
    }

    public async Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message == null)
            return;

        message.MarkFailed(error);
    }
}