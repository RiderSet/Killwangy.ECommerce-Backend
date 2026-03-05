namespace GBastos.Casa_dos_Farelos.PedidoService.Api.Contracts;

public sealed class CriarPedidoRequest
{
    public Guid ClienteId { get; set; }
    public decimal Valor { get; set; } = 0;
}