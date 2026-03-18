using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

internal sealed class ClienteInterno : Cliente
{
    public ClienteInterno(
        Guid id,
        string nome,
        string telefone,
        string email,
        TipoCliente tipoCliente,
        string documento)
        : base(id, nome, telefone, email, tipoCliente, documento)
    {
        ValidateInvariants();
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new ArgumentException("O Id do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(Telefone))
            throw new ArgumentException("O telefone é obrigatório.");

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            throw new ArgumentException("O email é inválido.");

        if (string.IsNullOrWhiteSpace(Documento))
            throw new ArgumentException("O documento é obrigatório.");

        if (TipoCliente != TipoCliente.Interno)
            throw new ArgumentException("ClienteInterno deve possuir TipoCliente = Interno.");
    }
}