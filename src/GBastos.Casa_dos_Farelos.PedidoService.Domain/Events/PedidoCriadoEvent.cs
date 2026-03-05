using GBastos.Casa_dos_Farelos.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.Events;

public sealed record PedidoCriadoEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Valor
) : DomainEventBase;