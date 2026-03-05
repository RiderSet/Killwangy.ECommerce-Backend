using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Services;

public sealed class ClientePessoaFisica : Cliente
{
    private ClientePessoaFisica(
        Guid id,
        string nome,
        string telefone,
        string email)
        : base(id, nome, telefone, email)
    {
        ValidateInvariants();
    }

    public static ClientePessoaFisica Criar(
        string nome,
        string telefone,
        string email)
    {
        return new ClientePessoaFisica(
            Guid.NewGuid(),
            nome,
            telefone,
            email);
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id do cliente inválido.");

        if (!Email.Contains("@"))
            throw new DomainException("Email inválido.");

        if (Telefone.Length < 10)
            throw new DomainException("Telefone inválido.");
    }
}