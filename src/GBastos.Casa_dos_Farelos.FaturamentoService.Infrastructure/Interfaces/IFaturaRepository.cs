using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;

public interface IFaturaRepository
{
    Task AddAsync(Fatura fatura, CancellationToken cancellationToken);
    Task<Fatura?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Fatura>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task UpdateAsync(Fatura fatura, CancellationToken cancellationToken);
    Task DeleteAsync(Fatura fatura, CancellationToken cancellationToken);
}