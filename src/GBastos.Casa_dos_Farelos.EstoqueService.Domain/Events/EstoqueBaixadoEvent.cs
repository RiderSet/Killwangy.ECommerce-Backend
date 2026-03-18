using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class EstoqueBaixadoEvent : IIntegrationEvent
{
    public Guid ProdutoId { get; }
    public int Quantidade { get; }

    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    public string EventType => nameof(EstoqueBaixadoEvent);

    public int Version => 1;

    public EstoqueBaixadoEvent(
        Guid produtoId,
        int quantidade)
    {
        if (produtoId == Guid.Empty)
            throw new ArgumentException("Produto inválido.", nameof(produtoId));

        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.", nameof(quantidade));

        ProdutoId = produtoId;
        Quantidade = quantidade;
    }
}