using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public sealed record EstoqueBaixadoDomainEvent(
    Guid AggregateId,
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : DomainEvent(AggregateId, null, null, null);