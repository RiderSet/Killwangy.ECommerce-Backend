using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.Events;

public sealed record EstoqueBaixadoDomainEvent(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade
) : DomainEvent(ProdutoId);