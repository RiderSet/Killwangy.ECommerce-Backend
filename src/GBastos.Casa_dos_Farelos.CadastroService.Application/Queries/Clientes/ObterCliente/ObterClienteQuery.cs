using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ObterCliente;

public record ObterClienteQuery(Guid ClienteId) : IRequest<ClienteDto?>;