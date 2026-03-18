using GBastos.Casa_dos_Farelos.CatalogoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly CatalogoDbContext _context;

    public ProdutoRepository(CatalogoDbContext context)
    {
        _context = context;
    }

    public async Task<Produto?> GetByIdAsync(Guid id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
    }

    public Task UpdateAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        return Task.CompletedTask;
    }
}