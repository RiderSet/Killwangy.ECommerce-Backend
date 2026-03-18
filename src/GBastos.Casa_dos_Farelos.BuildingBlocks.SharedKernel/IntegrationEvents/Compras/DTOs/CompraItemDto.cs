namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Compras.DTOs;

public class CompraItemDto
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal CustoUnitario { get; set; }
    public decimal SubTotal { get; set; }
}