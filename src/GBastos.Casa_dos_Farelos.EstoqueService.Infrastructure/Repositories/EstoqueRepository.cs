using GBastos.Casa_dos_Farelos.EstoqueService.Application.DTOs;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

public class EstoqueRepository : IEstoqueRepository
{
    private readonly EstoqueDbContext _context;
    public int QuantidadeReservada { get; private set; }
    public DateTime? ReservaExpiraEmUtc { get; private set; }

    public EstoqueRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<ProdutoEstoque?> GetByProdutoIdAsync(Guid produtoId)
    {
        return await _context.ProdutoEstoque
            .FirstOrDefaultAsync(x => x.Id == produtoId);
    }

    public async Task<List<OutboxMessageDto>> GetUnprocessedAsync(
        int take,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        return await _context.OutboxMessages
            .Where(x =>
                x.ProcessedOnUtc == null &&
                (x.LockedUntilUtc == null ||
                 x.LockedUntilUtc < now))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .Select(x => new OutboxMessageDto
            {
                Id = x.Id,
                Type = x.Type,
                Payload = x.Payload,
                OccurredOnUtc = x.OccurredOnUtc
            })
            .ToListAsync(ct);
    }

    public Task UpdateAsync(ProdutoEstoque estoque)
    {
        _context.ProdutoEstoque.Update(estoque);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task ExpireReservasAsync()
    {
        var now = DateTime.UtcNow;

        await _context.Produtos
            .Where(x =>
                x.QuantidadeReservada > 0 &&
                x.ReservaExpiraEmUtc != null &&
                x.ReservaExpiraEmUtc < now)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(p => p.QuantidadeDisponivel,
                    p => p.QuantidadeDisponivel + p.QuantidadeReservada)
                 .SetProperty(p => p.QuantidadeReservada, 0)
                 .SetProperty(p => p.ReservaExpiraEmUtc, (DateTime?)null));
    }
}