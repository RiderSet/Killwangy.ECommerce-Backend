using GBastos.Casa_dos_Farelos.CatalogoService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Interfaces;

public interface IProdutoRepository
{
    Task<Produto?> GetByIdAsync(Guid id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
}