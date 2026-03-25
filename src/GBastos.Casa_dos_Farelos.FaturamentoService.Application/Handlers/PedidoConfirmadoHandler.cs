using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Consume;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class PedidoConfirmadoHandler : INotificationHandler<PedidoConfirmadoEvent>
{
    private readonly ILogger<PedidoConfirmadoHandler> _logger;
    private readonly IFaturaRepository _faturaRepository;
    private readonly IEventPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;

    public PedidoConfirmadoHandler(
        ILogger<PedidoConfirmadoHandler> logger,
        IFaturaRepository faturaRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher publisher)
    {
        _logger = logger;
        _faturaRepository = faturaRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(PedidoConfirmadoEvent notification, CancellationToken cancellationToken)
    {
        var fatura = GerarFatura(notification);

        await Persistir(fatura, cancellationToken);
        await PublicarFaturaEmitidaEvent(fatura, cancellationToken);
    }

    private Fatura GerarFatura(PedidoConfirmadoEvent pedido)
    {
        _logger.LogInformation("Gerando fatura para pedido {PedidoId}", pedido.PedidoId);

        var numeroFatura = "FAT-" + DateTime.UtcNow.Ticks;
        var dtVencimento = DateTime.UtcNow.AddDays(7);

        var fatura = new Fatura(
            numero: numeroFatura,
            clienteId: pedido.ClienteId,
            pedidoId: pedido.PedidoId,
            dataVencimento: dtVencimento
        );

        return fatura;
    }

    private async Task Persistir(Fatura fatura, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Persistindo fatura {FaturaId}", fatura.Id);

        await _faturaRepository.AddAsync(fatura, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task PublicarFaturaEmitidaEvent(Fatura fatura, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publicando evento FaturaEmitida {FaturaId}", fatura.Id);

        var payload = JsonSerializer.Serialize(new
        {
            fatura.Id,
            fatura.PedidoId,
            fatura.ClienteId,
            fatura.ValorTotal,
            fatura.DataEmissao
        });

        await _publisher.PublishAsync(
            routingKey: "fatura.emitida",
            payload: payload,
            messageId: Guid.NewGuid(),
            eventType: nameof(FaturaEmitidaEvent),
            occurredOnUtc: DateTime.UtcNow,
            version: 1,
            cancellationToken: cancellationToken
        );
    }
}