namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IEstoqueUnitOfWork
{
    IReservaRepository Reservas { get; }
    IIdempotencyRepository Idempotency { get; }
    IOutboxRepository Outbox { get; }

    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}