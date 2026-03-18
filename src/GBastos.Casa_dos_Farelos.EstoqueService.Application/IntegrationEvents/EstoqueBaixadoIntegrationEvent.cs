using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.IntegrationEvents;

public sealed record EstoqueBaixadoIntegrationEvent(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : IntegrationEvent
{}