namespace GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;

public class ItemCompraDto
{
    public Guid ProdutoId { get; init; }
    public int Quantidade { get; init; }
    public decimal PrecoUnitario { get; init; }
}
