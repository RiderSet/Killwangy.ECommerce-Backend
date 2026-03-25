using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Persistence.Context;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class PagamentoConfirmadoHandler
    : INotificationHandler<PagamentoConfirmadoEvent>
{
    private readonly FaturamentoDbContext _context;
    private readonly IMediator _mediator;

    public PagamentoConfirmadoHandler(
        FaturamentoDbContext context,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(
        PagamentoConfirmadoEvent notification,
        CancellationToken cancellationToken)
    {
        var fatura = GerarFatura(notification);

        await Persistir(fatura, cancellationToken);

        await PublicarFaturaEmitidaEvent(fatura, cancellationToken);
    }

    private Fatura GerarFatura(PagamentoConfirmadoEvent notification)
    {
        var numero = $"FAT-{DateTime.UtcNow:yyyyMMddHHmmss}";

        var fatura = new Fatura(numero);

        // exemplo simples: valor total vindo do pagamento
        fatura.AdicionarItem(
            notification.PedidoId,
            1,
            notification.Valor);

        fatura.Emitir();

        return fatura;
    }

    private async Task Persistir(
        Fatura fatura,
        CancellationToken cancellationToken)
    {
        await _context.Faturas.AddAsync(fatura, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task PublicarFaturaEmitidaEvent(
        Fatura fatura,
        CancellationToken cancellationToken)
    {
        var evento = new FaturaEmitidaEvent(
            fatura.Id,
            fatura.Numero,
            fatura.ValorTotal,
            DateTime.UtcNow);

        await _mediator.Publish(evento, cancellationToken);
    }
}