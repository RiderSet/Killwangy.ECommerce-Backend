using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.CriarCliente;

public sealed class CriarClienteCommandHandler
    : IRequestHandler<CriarClienteCommand, Guid>
{
    private readonly IClienteRepository _clienteRepository;

    public CriarClienteCommandHandler(
        IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<Guid> Handle(
        CriarClienteCommand request,
        CancellationToken cancellationToken)
    {
        var existeDocumento = await _clienteRepository
            .ExistePorDocumentoAsync(request.Documento, cancellationToken);

        if (existeDocumento)
            throw new InvalidOperationException(
                "Cliente já cadastrado com este documento.");

        var existeEmail = await _clienteRepository
            .EmailJaExisteAsync(request.Email, cancellationToken);

        if (existeEmail)
            throw new InvalidOperationException(
                "Email já cadastrado.");

        var cliente = Cliente.Criar(
            request.Nome,
            request.Telefone,
            request.Email,
            request.TipoCliente,
            request.Documento);

        await _clienteRepository
            .AdicionarAsync(cliente, cancellationToken);

        await _clienteRepository
            .SaveChangesAsync(cancellationToken);

        return cliente.Id;
    }
}