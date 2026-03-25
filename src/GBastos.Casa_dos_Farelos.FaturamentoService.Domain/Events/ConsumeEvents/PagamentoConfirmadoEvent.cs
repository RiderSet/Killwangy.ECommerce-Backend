using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;

public record PagamentoConfirmadoEvent(
    Guid PedidoId,
    Guid PagamentoId,
    decimal Valor,
    DateTime DataConfirmacao
) : INotification;