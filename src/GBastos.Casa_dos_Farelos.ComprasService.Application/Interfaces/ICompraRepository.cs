using GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;

public interface ICompraRepository
{
    Task<Compra?> GetByIdAsync(
        Guid id,
        CancellationToken ct);

    Task<List<Compra>> GetAllAsync(
        CancellationToken ct);

    Task AddAsync(
        Compra compra,
        CancellationToken ct);

    void Update(
        Compra compra);

    void Remove(
        Compra compra);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken ct);
    Task<object?> ListAsync(CancellationToken ct);
}