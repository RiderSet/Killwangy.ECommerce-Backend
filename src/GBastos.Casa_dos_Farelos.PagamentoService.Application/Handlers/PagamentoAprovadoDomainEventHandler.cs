using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Pagamentos;
using GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Handlers;

public sealed class PagamentoAprovadoDomainEventHandler
    : INotificationHandler<PagamentoAprovadoEvent>
{
    private readonly IIntegrationEventOutbox _outbox;

    public PagamentoAprovadoDomainEventHandler(
        IIntegrationEventOutbox outbox)
    {
        _outbox = outbox;
    }

    public async Task Handle(
        PagamentoAprovadoEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new PagamentoAprovadoIntegrationEvent
        {
            PagamentoId = notification.PagamentoId,
            PedidoId = notification.PedidoId,
            ClienteId = notification.ClienteId,
            Valor = notification.ValorPg,
            OccurredOnUtc = notification.OccurredOnUtc
        };

        await _outbox.AddAsync(integrationEvent, cancellationToken);
    }
}