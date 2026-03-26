using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ObterCliente;

public sealed record ObterClienteQuery(Guid Id)
    : IRequest<ClienteDto?>;