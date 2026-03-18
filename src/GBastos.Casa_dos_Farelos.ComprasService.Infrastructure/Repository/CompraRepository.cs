using GBastos.Casa_dos_Farelos.ComprasService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Repository;

public sealed class CompraRepository : ICompraRepository
{
    private readonly ComprasDbContext _context;

    public CompraRepository(ComprasDbContext context)
    {
        _context = context;
    }

    public async Task<Compra?> GetByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        return await _context.Compras
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Compra>> GetAllAsync(
        CancellationToken ct)
    {
        return await _context.Compras
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(
        Compra compra,
        CancellationToken ct)
    {
        await _context.Compras.AddAsync(compra, ct);
    }

    public void Update(
        Compra compra)
    {
        _context.Compras.Update(compra);
    }

    public void Remove(
        Compra compra)
    {
        _context.Compras.Remove(compra);
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken ct)
    {
        return await _context.Compras
            .AnyAsync(x => x.Id == id, ct);
    }

    public async Task<List<Compra>> ListAsync(CancellationToken ct)
    {
        return await _context.Compras
            .AsNoTracking()
            .ToListAsync(ct);
    }
}