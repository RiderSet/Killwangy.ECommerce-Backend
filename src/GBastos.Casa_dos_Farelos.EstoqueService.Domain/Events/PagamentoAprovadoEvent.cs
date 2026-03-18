using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record PagamentoAprovadoEvent(
    Guid PedidoId,
    Guid ProdutoId,
    int Quantidade
) : IDomainEvent, INotification
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid AggregateId => PedidoId;
    public string EventType => nameof(PagamentoAprovadoEvent);
}