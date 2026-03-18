namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken = default);
}
