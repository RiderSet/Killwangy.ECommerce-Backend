using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class EstoqueBaixadoDomainEvent : DomainEvent
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public int Quantidade { get; }

    public EstoqueBaixadoDomainEvent(
        Guid produtoId,
        string nomeProduto,
        int quantidade)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
    }
}