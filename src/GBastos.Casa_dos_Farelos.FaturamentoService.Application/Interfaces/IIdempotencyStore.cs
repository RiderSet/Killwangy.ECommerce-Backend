namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;

public interface IIdempotencyStore
{
    Task<bool> ExistsAsync(string key);
    Task SetAsync(string key);
}