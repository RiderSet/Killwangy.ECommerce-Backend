using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IProdutoRepository
{
    Task<Produto?> GetByIdAsync(Guid id);
    void Add(Produto produto);
    Task SaveChangesAsync(CancellationToken ct);
}