using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;

public record PedidoConfirmadoEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal ValorTotal,
    DateTime DataConfirmacao
) : INotification;