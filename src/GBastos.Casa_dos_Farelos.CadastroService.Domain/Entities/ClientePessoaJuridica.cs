using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Utils;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Services;

public sealed class ClientePessoaJuridica : Cliente
{
    private ClientePessoaJuridica(
        Guid id,
        string nome,
        string telefone,
        string email,
        Cnpj cnpj)
        : base(id, nome, telefone, email, TipoCliente.PessoaJuridica, cnpj.Numero)
    {
        ValidateInvariants();
    }

    public static ClientePessoaJuridica Criar(
        string nome,
        string telefone,
        string email,
        string cnpjString)
    {
        var cnpj = Cnpj.Criar(cnpjString);

        return new ClientePessoaJuridica(
            Guid.NewGuid(),
            nome,
            telefone,
            email,
            cnpj);
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