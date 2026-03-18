namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken = default);
}
