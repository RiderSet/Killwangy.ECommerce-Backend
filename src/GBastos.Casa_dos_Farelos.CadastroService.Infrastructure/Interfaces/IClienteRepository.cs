using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IClienteRepository
{
   // Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task AdicionarAsync(
        Cliente cliente,
        CancellationToken cancellationToken);

    Task<bool> EmailJaExisteAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> ExistePorDocumentoAsync(
        string documento,
        CancellationToken cancellationToken);

    void Remover(Cliente cliente);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);

    Task<Cliente?> GetByIdAsync(Guid clienteId, CancellationToken cancellationToken);

    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
}