using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public class PedidoCanceladoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }

    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;

    public Guid AggregateId => PedidoId;
    public string EventType => nameof(PedidoCanceladoEvent);

    public PedidoCanceladoEvent(Guid pedidoId, Guid clienteId)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
    }
}