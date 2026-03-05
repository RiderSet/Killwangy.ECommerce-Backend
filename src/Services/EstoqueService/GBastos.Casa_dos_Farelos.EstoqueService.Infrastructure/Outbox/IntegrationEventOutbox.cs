using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pagamentos;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(integrationEvent, cancellationToken);
    }
}