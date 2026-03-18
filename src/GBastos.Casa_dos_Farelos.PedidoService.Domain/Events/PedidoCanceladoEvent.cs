using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public class PedidoCanceladoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public PedidoNumero Numero { get; }

    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;

    public Guid AggregateId => PedidoId;
    public string EventType => nameof(PedidoCanceladoEvent);

    public PedidoCanceladoEvent(Guid pedidoId, Guid clienteId, PedidoNumero numero)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Numero = numero;
    }
}