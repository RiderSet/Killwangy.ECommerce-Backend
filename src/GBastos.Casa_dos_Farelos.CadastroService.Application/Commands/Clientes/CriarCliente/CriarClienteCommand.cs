using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.CriarCliente;

public sealed class CriarClienteCommand : IRequest<Guid>
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Nome { get; }
    public string Telefone { get; } = string.Empty;
    public string Email { get; }
    public string Documento { get; }
    public TipoCliente TipoCliente { get; }

    public CriarClienteCommand(
        string nome,
        string telefone,
        string email,
        string documento,
        TipoCliente tipoCliente)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Documento = documento;
        TipoCliente = tipoCliente;
    }

    public CriarClienteCommand(string nome, string email, string documento)
    {
        Nome = nome;
        Email = email;
        Documento = documento;
    }
}