using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pagamentos;

namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}