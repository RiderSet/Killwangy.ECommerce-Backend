namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken = default);
}
