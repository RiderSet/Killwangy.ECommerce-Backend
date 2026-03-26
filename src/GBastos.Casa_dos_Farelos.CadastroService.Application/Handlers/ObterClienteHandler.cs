using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ObterCliente;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Handlers;

public sealed class ObterClienteHandler
    : IRequestHandler<ObterClienteQuery, ClienteDto?>
{
    private readonly IClienteRepository _repository;

    public ObterClienteHandler(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClienteDto?> Handle(
        ObterClienteQuery request,
        CancellationToken cancellationToken)
    {
        var cliente = await _repository
            .ObterPorIdAsync(request.Id, cancellationToken);

        if (cliente is null)
            return null;

        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Documento = cliente.Documento
        };
    }
}