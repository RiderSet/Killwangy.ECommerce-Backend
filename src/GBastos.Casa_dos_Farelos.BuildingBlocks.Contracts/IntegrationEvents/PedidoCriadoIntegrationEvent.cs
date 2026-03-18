namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Contracts.IntegrationEvents;

public record PedidoCriadoIntegrationEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Valor);