using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;

public sealed record PagamentoAprovadoIntegrationEvent :
    IntegrationEvent
{
    public Guid PagamentoId { get; init; }
    public Guid PedidoId { get; init; }
    public Guid ClienteId { get; init; }
    public decimal Valor { get; init; }
    //public override string EventType =>
    //    nameof(PagamentoAprovadoIntegrationEvent);

}