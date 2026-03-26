using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

public sealed class ClientePJ : Cliente
{
    private ClientePJ() { }

    public ClientePJ(
        Guid id,
        string nome,
        string telefone,
        string email,
        string cnpj)
        : base(id, nome, telefone, email, TipoCliente.PessoaJuridica, cnpj)
    {
    }

    protected override void ValidateInvariants()
    {
        if (Cnpj is null)
            throw new DomainException("Cliente PJ deve possuir CNPJ");

        if (Cpf is not null)
            throw new DomainException("Cliente PJ não pode possuir CPF");

        _ = Cnpj.Numero;
    }
}