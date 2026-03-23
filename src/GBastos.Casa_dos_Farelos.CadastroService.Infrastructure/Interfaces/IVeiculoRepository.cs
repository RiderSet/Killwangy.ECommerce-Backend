using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IVeiculoRepository
{
    Task<Veiculo?> GetByIdAsync(
        Guid Id,
        CancellationToken ct = default);

    Task<Veiculo?> GetByPlacaAsync(
        string placa,
        CancellationToken ct = default);

    Task<List<Veiculo>> GetByProprietarioIdAsync(
        Guid clienteId,
        CancellationToken ct = default);

    Task AddAsync(
        Veiculo veiculo,
        CancellationToken ct = default);

    Task RemoveAsync(
        Veiculo veiculo,
        CancellationToken ct = default);

    Task<bool> ExistsPlacaAsync(
        string placa,
        CancellationToken ct = default);

    Task SaveChangesAsync(
        CancellationToken ct = default);
}