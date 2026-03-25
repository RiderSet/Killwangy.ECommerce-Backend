using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class PedidoConfirmadoHandler
    : INotificationHandler<PedidoConfirmadoEvent>
{
    public async Task Handle(
        PedidoConfirmadoEvent notification,
        CancellationToken cancellationToken)
    {
        // gerar fatura
        // persistir
        // publicar FaturaEmitidaEvent
    }
}