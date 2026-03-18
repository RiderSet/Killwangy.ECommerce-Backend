using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;

public sealed record PagamentoRecusadoIntegrationEvent : IntegrationEvent
{
    public Guid PagamentoId { get; }
    public Guid PedidoId { get; }
    public string Motivo { get; }

    public PagamentoRecusadoIntegrationEvent(
        Guid pagamentoId,
        Guid pedidoId,
        string motivo)
    {
        if (pagamentoId == Guid.Empty)
            throw new ArgumentException("PagamentoId inválido.");

        if (pedidoId == Guid.Empty)
            throw new ArgumentException("PedidoId inválido.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Motivo obrigatório.");

        PagamentoId = pagamentoId;
        PedidoId = pedidoId;
        Motivo = motivo;
    }
}