using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.UOW;

public sealed class EstoqueUnitOfWork : IEstoqueUnitOfWork
{
    private readonly EstoqueDbContext _context;
    private IDbContextTransaction? _transaction;

    public EstoqueUnitOfWork(
        EstoqueDbContext context,
        IReservaRepository reservas,
        IIdempotencyRepository idempotency,
        IOutboxRepository outbox)
    {
        _context = context;

        Reservas = reservas;
        Idempotency = idempotency;
        Outbox = outbox;
    }

    public IReservaRepository Reservas { get; }
    public IOutboxRepository Outbox { get; }
    public IIdempotencyRepository Idempotency { get; }

    IOutboxRepositoryES IEstoqueUnitOfWork.Outbox => throw new NotImplementedException();

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        if (_transaction == null)
            return;

        await _context.SaveChangesAsync(ct);
        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}