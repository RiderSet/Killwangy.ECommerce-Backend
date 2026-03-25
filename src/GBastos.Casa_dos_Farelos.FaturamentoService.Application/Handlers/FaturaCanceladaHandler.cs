using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Events;

public sealed class FaturaCanceladaHandler
    : INotificationHandler<FaturaCanceladaEvent>
{
    private readonly ILogger<FaturaCanceladaHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IEventPublisher _publisher;

    public FaturaCanceladaHandler(
        ILogger<FaturaCanceladaHandler> logger,
        IDistributedCache cache,
        IEventPublisher publisher)
    {
        _logger = logger;
        _cache = cache;
        _publisher = publisher;
    }

    public async Task Handle(
        FaturaCanceladaEvent notification,
        CancellationToken cancellationToken)
    {
        await Log(notification);

        await InvalidarCache(notification, cancellationToken);

        await EnviarIntegracao(notification, cancellationToken);
    }

    private Task Log(FaturaCanceladaEvent notification)
    {
        _logger.LogWarning(
            "Fatura cancelada {Numero}",
            notification.Numero);

        return Task.CompletedTask;
    }

    private async Task InvalidarCache(
        FaturaCanceladaEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(
            $"fatura:{notification.FaturaId}",
            cancellationToken);
    }

    private async Task EnviarIntegracao(
        FaturaCanceladaEvent notification,
        CancellationToken cancellationToken)
    {
        // Construa o payload (por exemplo JSON)
        var payload = System.Text.Json.JsonSerializer.Serialize(new
        {
            notification.FaturaId,
            notification.Numero,
            notification.Motivo
        });

        // Defina o routingKey e eventType conforme padrão do seu sistema
        string routingKey = "fatura.cancelada";
        string eventType = nameof(FaturaCanceladaEvent);

        await _publisher.PublishAsync(
            routingKey: routingKey,
            payload: payload,
            messageId: notification.MessageId,      // precisa existir no evento
            eventType: eventType,
            occurredOnUtc: notification.OccurredOnUtc,
            version: 1,                              // defina versão do evento
            cancellationToken: cancellationToken
        );

        _logger.LogInformation(
            "Evento publicado: {RoutingKey} {Numero}",
            routingKey,
            notification.Numero);
    }
}