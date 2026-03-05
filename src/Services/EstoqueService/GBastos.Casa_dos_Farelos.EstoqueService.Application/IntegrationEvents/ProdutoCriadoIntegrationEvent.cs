using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.General;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.IntegrationEvents;

public sealed record ProdutoCriadoIntegrationEvent(Guid ProdutoId, decimal PrecoVenda) : IntegrationEvent
{
    public string Nome { get; init; } = default!;
}