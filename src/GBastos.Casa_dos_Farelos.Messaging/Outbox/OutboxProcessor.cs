using GBastos.Casa_dos_Farelos.Messaging.Abstractions;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Messaging.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IOutboxRepository _repository;
    private readonly IEventBus _bus;

    public OutboxProcessor(
        IOutboxRepository repository,
        IEventBus bus)
    {
        _repository = repository;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Pega mensagens pendentes
            var messages = await _repository.GetPendingAsync(50, stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    // Descobre o tipo do evento
                    var eventType = Type.GetType(message.EventType);
                    if (eventType == null)
                    {
                        await _repository.MarkAsFailedAsync(message.Id, "Tipo do evento não encontrado", stoppingToken);
                        continue;
                    }

                    // Desserializa o payload para o tipo correto
                    var @event = JsonSerializer.Deserialize(
                        message.Payload,
                        eventType);

                    if (@event == null)
                    {
                        await _repository.MarkAsFailedAsync(message.Id, "Falha ao desserializar o payload", stoppingToken);
                        continue;
                    }

                    // Publica o evento no EventBus
                    await _bus.PublishAsync((IIntegrationEvent)@event, stoppingToken);

                    // Marca como processado
                    await _repository.MarkAsProcessedAsync(message.Id, stoppingToken);
                }
                catch (Exception ex)
                {
                    // Marca como falha no repositório
                    await _repository.MarkAsFailedAsync(message.Id, ex.Message, stoppingToken);
                }
            }

            // Delay entre batches
            await Task.Delay(2000, stoppingToken);
        }
    }
}