using GBastos.Casa_dos_Farelos.CadastroService.Domain.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public sealed class Usuario : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }

    private Usuario() : base(Guid.Empty) { }

    private Usuario(Guid id, string nome, string email, string senhaHash)
        : base(id)
    {
        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        SenhaHash = senhaHash;
        Ativo = true;

        ValidateInvariants();

        AddDomainEvent(new UsuarioCriadoDomainEvent(Id, Nome, Email));
    }

    public static Usuario Criar(string nome, string email, string senhaHash)
    {
        return new Usuario(Guid.NewGuid(), nome, email, senhaHash);
    }

    public void AlterarNome(string nome)
    {
        Nome = nome.Trim();

        ValidateInvariants();

        AddDomainEvent(new UsuarioAtualizadoDomainEvent(Id, Nome, Email));
    }

    public void AlterarEmail(string email)
    {
        Email = email.Trim().ToLowerInvariant();

        ValidateInvariants();

        AddDomainEvent(new UsuarioAtualizadoDomainEvent(Id, Nome, Email));
    }

    public void Desativar()
    {
        if (!Ativo)
            throw new DomainException("Usuário já está inativo.");

        Ativo = false;

        AddDomainEvent(new UsuarioDesativadoDomainEvent(Id));
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id do usuário inválido.");

        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainException("Email é obrigatório.");

        if (!Email.Contains("@"))
            throw new DomainException("Email inválido.");

        if (string.IsNullOrWhiteSpace(SenhaHash))
            throw new DomainException("Senha é obrigatória.");
    }
}