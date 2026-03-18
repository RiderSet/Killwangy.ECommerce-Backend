using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueAtualizadoDomainEvent : DomainEvent
{
    public Guid ProdutoId { get; }
    public int QuantidadeEstoque { get; }

    public EstoqueAtualizadoDomainEvent(
        Guid produtoId,
        int quantidadeEstoque)
        : base(produtoId)
    {
        ProdutoId = produtoId;
        QuantidadeEstoque = quantidadeEstoque;
    }
}