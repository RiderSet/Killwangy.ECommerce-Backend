using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.AtualizarCliente;

public sealed record AtualizarClienteCommand(
    Guid Id,
    string Nome,
    string Telefone,
    string Email
) : IRequest<Unit>;