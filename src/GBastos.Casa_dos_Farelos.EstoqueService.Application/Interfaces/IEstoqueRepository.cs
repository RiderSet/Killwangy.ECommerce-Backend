using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IEstoqueRepository
{
    Task<ProdutoEstoque?> GetByProdutoIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ProdutoEstoque estoqueProduto, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProdutoEstoque estoqueProduto, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExpireReservasAsync(CancellationToken cancellationToken = default);
}