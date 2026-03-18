using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;

namespace GBastos.Casa_dos_Farelos.PedidoService.Api.Contracts;

public sealed class CriarPedidoRequest
{
    public Guid ClienteId { get; set; }
    public Money Valor { get; set; } = 0;
}