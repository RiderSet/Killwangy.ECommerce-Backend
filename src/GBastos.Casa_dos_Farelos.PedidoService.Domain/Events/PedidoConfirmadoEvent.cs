using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public class PedidoConfirmadoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public PedidoNumero Numero { get; }
    public Money Total { get; }

    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;

    public Guid AggregateId => PedidoId;
    public string EventType => nameof(PedidoCanceladoEvent);

    public PedidoConfirmadoEvent(
        Guid pedidoId,
        Guid clienteId,
        Money total,
        PedidoNumero numero)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
        Numero = numero;
    }
}