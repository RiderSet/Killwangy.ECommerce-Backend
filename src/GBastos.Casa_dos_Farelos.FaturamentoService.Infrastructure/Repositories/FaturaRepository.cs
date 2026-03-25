using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Repositories;

public class FaturaRepository : IFaturaRepository
{
    private readonly FaturamentoDbContext _context;

    public FaturaRepository(FaturamentoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Fatura fatura, CancellationToken cancellationToken)
    {
        await _context.Faturas.AddAsync(fatura, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Fatura?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Faturas.FindAsync(new object?[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Fatura>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Faturas
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Fatura fatura, CancellationToken cancellationToken)
    {
        _context.Faturas.Update(fatura);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Fatura fatura, CancellationToken cancellationToken)
    {
        _context.Faturas.Remove(fatura);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task AddAsync(Fatura fatura)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(object fatura, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}