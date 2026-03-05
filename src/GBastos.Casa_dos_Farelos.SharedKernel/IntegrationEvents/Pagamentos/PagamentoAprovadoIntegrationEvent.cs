using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.General;

namespace GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.Pagamentos;

public sealed record PagamentoAprovadoIntegrationEvent :
    IntegrationEventBase
{
    public Guid PagamentoId { get; init; }
    public Guid PedidoId { get; init; }
    public Guid ClienteId { get; init; }
    public decimal Valor { get; init; }

    public override string EventType =>
        nameof(PagamentoAprovadoIntegrationEvent);
}