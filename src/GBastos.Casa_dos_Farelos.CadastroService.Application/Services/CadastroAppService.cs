using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Factories;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

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
        TipoCliente tipoCliente,
        string documento,
        CancellationToken ct)
    {
        var cliente = ClienteFactory.Criar(
            tipoCliente,
            nome,
            telefone,
            email,
            documento);

        var integrationEvent = new BuildingBlocks.SharedKernel.IntegrationEvents.Cadastros.ClienteCadastradoIntegrationEvent
        {
            ClienteId = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email
        };

        await _eventBus.PublishAsync(integrationEvent, ct);

        return cliente.Id;
    }
}