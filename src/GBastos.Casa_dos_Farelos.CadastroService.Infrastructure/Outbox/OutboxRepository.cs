using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly CadastroDbContext _context;

    public OutboxRepository(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<List<OutboxMessage>> GetPendingAsync(
        int take,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        return await _context.Set<OutboxMessage>()
            .Where(x =>
                !x.IsProcessed &&
                (x.LockedUntilUtc == null || x.LockedUntilUtc < now))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstAsync(x => x.Id == id, ct);

        message.MarkAsProcessed();

        await _context.SaveChangesAsync(ct);
    }

    public async Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstAsync(x => x.Id == id, ct);

        message.MarkFailed(error);

        await _context.SaveChangesAsync(ct);
    }

    public async Task LockAsync(
        Guid id,
        Guid lockId,
        TimeSpan duration,
        CancellationToken ct)
    {
        var message = await _context.Set<OutboxMessage>()
            .FirstAsync(x => x.Id == id, ct);

        message.Lock(lockId, duration);

        await _context.SaveChangesAsync(ct);
    }

    public async Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        var message = new OutboxMessage(
            integrationEvent.Id,
            integrationEvent.OccurredOnUtc,
            integrationEvent.GetType().Name,
            JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType())
        );

        await _context.Set<OutboxMessage>().AddAsync(message, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}