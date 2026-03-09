using GBastos.Casa_dos_Farelos.SharedKernel.Events;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

public sealed record PedidoCriadoEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Valor
) : DomainEventBase;