using GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class ReservaCanceladaDomainEvent : DomainEvent
{
    public Guid ProdutoId { get; }
    public Guid ReservaId { get; }

    public ReservaCanceladaDomainEvent(
        Guid produtoId,
        Guid reservaId)
        : base(produtoId)
    {
        ProdutoId = produtoId;
        ReservaId = reservaId;
    }
}