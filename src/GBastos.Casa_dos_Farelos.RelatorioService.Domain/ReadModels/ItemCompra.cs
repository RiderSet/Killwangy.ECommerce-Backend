using GBastos.Casa_dos_Farelos.RelatorioService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class ItemCompra : BaseEntity
{
    public Guid CompraId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public decimal CustoUnitario { get; private set; }
    public decimal SubTotal => Quantidade * CustoUnitario;

    protected ItemCompra() { }

    public ItemCompra(
        Guid compraId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal custoUnitario)
    {
        Id = Guid.NewGuid();
        CompraId = compraId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }
}