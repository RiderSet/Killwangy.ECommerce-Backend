using GBastos.Casa_dos_Farelos.CadastroService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Repositories;

public class VeiculoRepository : IVeiculoRepository
{
    private readonly CadastroDbContext _context;

    public VeiculoRepository(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<Veiculo?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    public async Task<Veiculo?> GetByPlacaAsync(string placa, CancellationToken ct = default)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Placa != null && v.Placa.ToString() == placa, ct);
    }

    public async Task<List<Veiculo>> GetByProprietarioIdAsync(Guid proprietarioId, CancellationToken ct = default)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .Where(v => v.ProprietarioId == proprietarioId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Veiculo veiculo, CancellationToken ct = default)
    {
        await _context.Veiculos.AddAsync(veiculo, ct);
    }

    public async Task RemoveAsync(Veiculo veiculo, CancellationToken ct = default)
    {
        _context.Veiculos.Remove(veiculo);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsPlacaAsync(string placa, CancellationToken ct = default)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .AnyAsync(v => v.Placa != null && v.Placa.ToString() == placa, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}