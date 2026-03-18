using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;

public sealed class PagamentoAprovadoDomainEvent : IDomainEvent
{
    public Guid PagamentoId { get; }
    public Guid PedidoId { get; }
    public decimal Valor { get; }

    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid AggregateId => PagamentoId;

    public string EventType => nameof(PagamentoAprovadoDomainEvent);

    public PagamentoAprovadoDomainEvent(
        Guid pagamentoId,
        Guid pedidoId,
        decimal valor)
    {
        if (pagamentoId == Guid.Empty)
            throw new ArgumentException("Pagamento inválido.", nameof(pagamentoId));

        if (pedidoId == Guid.Empty)
            throw new ArgumentException("Pedido inválido.", nameof(pedidoId));

        if (valor <= 0)
            throw new ArgumentException("Valor inválido.", nameof(valor));

        PagamentoId = pagamentoId;
        PedidoId = pedidoId;
        Valor = valor;
    }
}