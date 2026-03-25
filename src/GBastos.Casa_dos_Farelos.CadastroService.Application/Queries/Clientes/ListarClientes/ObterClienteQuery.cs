using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ListarClientes;

public sealed class ObterClienteQuery : IRequest<ClienteDto>
{
    public Guid ClienteId { get; }

    public ObterClienteQuery(Guid id)
    {
        ClienteId = id;
    }
}