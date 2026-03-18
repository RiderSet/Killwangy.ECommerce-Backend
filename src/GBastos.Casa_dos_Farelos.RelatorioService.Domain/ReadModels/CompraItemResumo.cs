namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class CompraItemResumo
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal CustoUnitario { get; set; }
    public decimal SubTotal => Quantidade * CustoUnitario;
}