using GBastos.Casa_dos_Farelos.CadastroService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pagamentos;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;


public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}