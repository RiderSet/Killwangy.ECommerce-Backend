using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.RemoverCliente;

public record RemoverClienteCommand(Guid Id) : IRequest<Unit>;