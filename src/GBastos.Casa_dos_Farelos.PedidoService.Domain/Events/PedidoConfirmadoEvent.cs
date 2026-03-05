using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public class PedidoConfirmadoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public Money Total { get; }

    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;

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