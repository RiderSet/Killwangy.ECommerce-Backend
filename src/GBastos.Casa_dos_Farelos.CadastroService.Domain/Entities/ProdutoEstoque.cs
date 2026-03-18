namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

public class ProdutoEstoque
{
    public Guid ProdutoId { get; set; }

    public int QuantidadeDisponivel { get; set; }
    public int QuantidadeReservada { get; set; }

    public DateTime? ReservaExpiraEmUtc { get; set; }
}