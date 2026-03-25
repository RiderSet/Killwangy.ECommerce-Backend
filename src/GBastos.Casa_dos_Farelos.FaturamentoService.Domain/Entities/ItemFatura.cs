namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Entities;

public class ItemFatura
{
    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    public decimal Total => Quantidade * ValorUnitario;

    public ItemFatura(
        Guid produtoId,
        int quantidade,
        decimal valorUnitario)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }
}