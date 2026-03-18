using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.DomainEvents;

public sealed class FornecedorAtualizadoDomainEvent : IDomainEvent
{
    public Guid FornecedorId { get; }
    public string Nome { get; }
    public string Telefone { get; }
    public string Email { get; }
    public string CNPJ { get; }

    public DateTime OccurredOnUtc { get; }

    public Guid EventId => Guid.NewGuid();

    public Guid AggregateId => Guid.NewGuid();

    public string EventType => throw new NotImplementedException();

    public FornecedorAtualizadoDomainEvent(
        Guid fornecedorId,
        string nome,
        string telefone,
        string email,
        string cnpj)
    {
        FornecedorId = fornecedorId;
        Nome = nome;
        Telefone = telefone;
        Email = email;
        CNPJ = cnpj;
        OccurredOnUtc = DateTime.UtcNow;
    }
}