using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

public sealed class ClientePF : Cliente
{
    private ClientePF() { }

    public ClientePF(
        Guid id,
        string nome,
        string telefone,
        string email,
        string cpf)
        : base(id, nome, telefone, email, TipoCliente.PessoaFisica, cpf)
    {
    }

    protected override void ValidateInvariants()
    {
        if (Cpf is null)
            throw new DomainException("Cliente PF deve possuir CPF");

        if (Cnpj is not null)
            throw new DomainException("Cliente PF não pode possuir CNPJ");

        _ = Cpf.Numero; 
    }
}