using GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.Interfaces;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

public class FaturaCanceladaHandler
    : INotificationHandler<FaturaCanceladaEvent>
{
    private readonly ILogger<FaturaCanceladaHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IRabbitMQPublisher _publisher;

    public FaturaCanceladaHandler(
        ILogger<FaturaCanceladaHandler> logger,
        IDistributedCache cache,
        IRabbitMQPublisher publisher)
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
        await _publisher.PublishAsync(
            exchange: "faturamento.events",
            routingKey: "fatura.cancelada",
            message: notification,
            cancellationToken);

        _logger.LogInformation(
            "Evento publicado RabbitMQ: fatura.cancelada {Numero}",
            notification.Numero);
    }
}