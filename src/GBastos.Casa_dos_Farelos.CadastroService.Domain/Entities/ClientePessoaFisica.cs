using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Utils;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Services;

public sealed class ClientePessoaFisica : Cliente
{
    private ClientePessoaFisica(
        Guid id,
        string nome,
        string telefone,
        string email,
        Cpf cpf)
        : base(id, nome, telefone, email, TipoCliente.PessoaFisica, cpf.Numero)
    {
        ValidateInvariants();
    }

    public static ClientePessoaFisica Criar(
        string nome,
        string telefone,
        string email,
        string cpfString)
    {
        var cpf = Cpf.Criar(cpfString);

        return new ClientePessoaFisica(
            Guid.NewGuid(),
            nome,
            telefone,
            email,
            cpf);
    }

    internal static object Criar(string nome, string telefone, string email)
    {
        throw new NotImplementedException();
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