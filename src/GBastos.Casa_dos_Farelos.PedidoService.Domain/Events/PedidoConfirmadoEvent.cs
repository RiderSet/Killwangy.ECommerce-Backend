using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public class PedidoConfirmadoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public Money Total { get; }

    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;

    public Guid AggregateId => PedidoId;
    public string EventType => nameof(PedidoCanceladoEvent);

    public PedidoConfirmadoEvent(
        Guid pedidoId,
        Guid clienteId,
        Money total)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
    }
}