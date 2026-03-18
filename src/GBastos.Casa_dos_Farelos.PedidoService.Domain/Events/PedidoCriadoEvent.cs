using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public sealed record PedidoCriadoEvent(
    Guid PedidoId,
    Guid ClienteId,
    Money Valor,
    PedidoNumero numero) : DomainEventBase;