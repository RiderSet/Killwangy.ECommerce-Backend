namespace GBastos.Casa_dos_Farelos.CatalogoService.Application.DTOs;

public class ProdutoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
}