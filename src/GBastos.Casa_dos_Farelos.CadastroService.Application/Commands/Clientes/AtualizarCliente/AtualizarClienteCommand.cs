using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.AtualizarCliente;

public sealed class AtualizarClienteCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Telefone { get; }
    public string Email { get; }

    public AtualizarClienteCommand(
        Guid id,
        string nome,
        string telefone,
        string email)
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Email = email;
    }

    public AtualizarClienteCommand(Guid id, string nome, string email)
    {
        Id = id;
        Nome = nome;
        Email = email;
    }
}