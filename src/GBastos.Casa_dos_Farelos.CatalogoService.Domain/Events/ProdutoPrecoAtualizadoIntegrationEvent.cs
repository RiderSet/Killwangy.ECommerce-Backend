namespace GBastos.Casa_dos_Farelos.CatalogoService.Domain.Events;

public sealed class ProdutoPrecoAtualizadoIntegrationEvent
{
    public Guid ProdutoId { get; init; }
    public decimal NovoPreco { get; init; }
    public DateTime AtualizadoEmUtc { get; init; }

    public ProdutoPrecoAtualizadoIntegrationEvent(
        Guid produtoId,
        decimal novoPreco)
    {
        ProdutoId = produtoId;
        NovoPreco = novoPreco;
        AtualizadoEmUtc = DateTime.UtcNow;
    }
}