using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.ObterCliente;

public sealed class ObterClienteQuery : IRequest<ClienteDto?>
{
    public Guid Id { get; }

    public ObterClienteQuery(Guid id)
    {
        Id = id;
    }
}