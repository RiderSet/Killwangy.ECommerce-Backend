using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

internal interface IIntegrationEventOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    Task<IEnumerable<IIntegrationEvent>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken = default);
}