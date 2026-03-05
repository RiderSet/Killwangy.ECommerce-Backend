using GBastos.Casa_dos_Farelos.CadastroService.Domain.Services;
using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Services;

public sealed class CadastroAppService
{
    private readonly IEventBus _eventBus;

    public CadastroAppService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task<Guid> CadastrarAsync(
        string nome,
        string telefone,
        string email,
        CancellationToken ct)
    {
        var cliente = ClientePessoaFisica.Criar(
            nome,
            telefone,
            email);

        var integrationEvent = new ClienteCadastradoIntegrationEvent
        {
            ClienteId = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email
        };

        await _eventBus.PublishAsync(integrationEvent, ct);

        return cliente.Id;
    }
}