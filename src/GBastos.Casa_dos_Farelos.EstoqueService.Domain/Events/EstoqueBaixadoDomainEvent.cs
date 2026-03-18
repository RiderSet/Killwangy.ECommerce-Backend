using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueBaixadoDomainEvent : DomainEvent
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public int Quantidade { get; }

    public EstoqueBaixadoDomainEvent(
        Guid aggregateId,
        Guid produtoId,
        string nomeProduto,
        int quantidade)
        : base(aggregateId, null, null, null)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
    }
}