using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly JsonSerializerOptions _jsonOptions;

    public OutboxProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessMessagesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessMessagesAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider
            .GetRequiredService<IOutboxRepository>();

        var eventBus = scope.ServiceProvider
            .GetRequiredService<IEventBus>();

        var messages = await repository.GetPendingAsync(
            take: 20,
            ct);

        foreach (var message in messages)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message.Payload))
                    throw new InvalidOperationException("Outbox message sem conteúdo.");

                var type = Type.GetType(message.Type);

                if (type is null)
                    throw new InvalidOperationException(
                        $"Tipo de evento não encontrado: {message.Type}");

                var integrationEvent = JsonSerializer.Deserialize(
                    message.Payload,
                    type,
                    _jsonOptions);

                if (integrationEvent is not IIntegrationEvent domainEvent)
                    throw new InvalidOperationException(
                        $"Evento inválido: {message.Type}");

                await eventBus.PublishAsync(
                    domainEvent,
                    ct);

                await repository.MarkAsProcessedAsync(
                    message.Id,
                    ct);
            }
            catch (Exception ex)
            {
                await repository.MarkAsFailedAsync(
                    message.Id,
                    ex.Message,
                    ct);
            }
        }
    }
}