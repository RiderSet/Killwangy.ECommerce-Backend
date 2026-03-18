using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<IIntegrationEvent>> GetPendingAsync(
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        Guid eventId,
        string error,
        CancellationToken cancellationToken = default);
    Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}