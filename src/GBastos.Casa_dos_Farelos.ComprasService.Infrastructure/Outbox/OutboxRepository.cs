using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;

public class OutboxRepository : IOutboxRepository
{
    private readonly ComprasDbContext _context;

    public OutboxRepository(ComprasDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken ct)
    {
        await _context.Set<OutboxMessage>().AddAsync(message, ct);
    }

    public async Task<List<OutboxMessage>> GetPendingAsync(CancellationToken ct)
    {
        return await _context.Set<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .ToListAsync(ct);
    }

    public async Task<List<OutboxMessage>> GetPendingAsync(int take, CancellationToken ct)
    {
        return await _context.Set<OutboxMessage>()
            .Where(x =>
                x.ProcessedOnUtc == null &&
                (x.LockedUntilUtc == null || x.LockedUntilUtc < DateTime.UtcNow))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task LockAsync(Guid id, Guid lockId, TimeSpan duration, CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.Lock(lockId, duration);

        _context.Update(message);
    }

    public async Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkFailed(error);

        _context.Update(message);
    }

    public async Task MarkAsProcessedAsync(Guid id, CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkAsProcessed();

        _context.Update(message);
    }
}