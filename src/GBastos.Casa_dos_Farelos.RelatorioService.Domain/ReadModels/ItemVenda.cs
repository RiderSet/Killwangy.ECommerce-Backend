using GBastos.Casa_dos_Farelos.RelatorioService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class ItemVenda : BaseEntity
{
    public Guid PedidoId { get; private set; }

    public Guid ProdutoId { get; private set; }

    public string NomeProduto { get; private set; } = string.Empty;

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    public decimal SubTotal => Quantidade * PrecoUnitario;

    protected ItemVenda() { }

    public ItemVenda(
        Guid pedidoId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal precoUnitario)
    {
        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public ItemVenda(Guid produtoId, int quantidade, decimal precoUnitario)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    internal void DefinirVenda(Guid id)
    {
        throw new NotImplementedException();
    }
}