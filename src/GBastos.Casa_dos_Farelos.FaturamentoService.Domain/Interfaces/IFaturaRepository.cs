using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Interfaces;

public interface IFaturaRepository
{
    Task<Fatura?> GetByIdAsync(Guid id);
    Task AddAsync(Fatura fatura);
    Task SaveChangesAsync();
}