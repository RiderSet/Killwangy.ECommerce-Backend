using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Utils;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public abstract class Cliente : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public TipoCliente TipoCliente { get; private set; }

    public Cpf? Cpf { get; private set; }
    public Cnpj? Cnpj { get; private set; }

    public string Documento
    {
        get
        {
            if (Cpf is not null)
                return Cpf.Numero;

            if (Cnpj is not null)
                return Cnpj.Numero;

            return string.Empty;
        }
    }

    protected Cliente() : base(Guid.Empty) { }

    protected Cliente(
        Guid id,
        string nome,
        string telefone,
        string email,
        TipoCliente tipoCliente,
        string documento)
        : base(id)
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
        SetTipoCliente(tipoCliente, documento);
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        Nome = nome.Trim();
    }

    private void SetTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone é obrigatório");

        Telefone = telefone.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email é obrigatório");

        Email = email.Trim();
    }

    private void SetTipoCliente(TipoCliente tipoCliente, string documento)
    {
        TipoCliente = tipoCliente;

        switch (tipoCliente)
        {
            case TipoCliente.PessoaFisica:
                Cpf = Cpf.Criar(documento);
                Cnpj = null;
                break;

            case TipoCliente.PessoaJuridica:
                Cnpj = Cnpj.Criar(documento);
                Cpf = null;
                break;

            default:
                throw new DomainException("Tipo de cliente inválido");
        }
    }

    public static Cliente Criar(string nome, string telefone, string email, TipoCliente tipoCliente, string documento)
    {
        throw new NotImplementedException();
    }
}