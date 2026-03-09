using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class EstoqueReservadoDomainEvent : DomainEvent
{
    public Guid ProdutoId { get; }
    public Guid PedidoId { get; }
    public int Quantidade { get; }

    public EstoqueReservadoDomainEvent(
        Guid produtoId,
        Guid pedidoId,
        int quantidade)
        : base(produtoId)
    {
        ProdutoId = produtoId;
        PedidoId = pedidoId;
        Quantidade = quantidade;
    }
}