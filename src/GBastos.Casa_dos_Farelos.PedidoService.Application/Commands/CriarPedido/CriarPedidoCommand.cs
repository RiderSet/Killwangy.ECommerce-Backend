namespace GBastos.Casa_dos_Farelos.PedidoService.Application.Commands.CriarPedido;

public class CriarPedidoCommand
{
    public CriarPedidoCommand(Guid clienteId)
    {
        ClienteId = clienteId;
    }

    public Guid ClienteId { get; internal set; }
}