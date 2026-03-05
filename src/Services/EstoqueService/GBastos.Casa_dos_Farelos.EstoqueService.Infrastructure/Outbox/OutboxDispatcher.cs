using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using MassTransit;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxDispatcher
{
    private readonly IOutboxRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OutboxDispatcher(
        IOutboxRepository repository,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        const int batchSize = 50;

        var messages = await _repository.GetUnprocessedAsync(batchSize, cancellationToken);

        foreach (var message in messages)
        {
            var lockUntil = DateTime.UtcNow.AddMinutes(1);

            var locked = await _repository.TryLockAsync(
                message.Id,
                lockUntil,
                cancellationToken);

            if (!locked)
                continue;

            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                    continue;

                var integrationEvent =
                    JsonSerializer.Deserialize(
                        message.Payload,
                        type);

                if (integrationEvent is null)
                    continue;

                await _publishEndpoint.Publish(
                    integrationEvent,
                    cancellationToken);

                await _repository.MarkAsProcessedAsync(
                    message.Id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await _repository.MarkAsFailedAsync(
                    message.Id,
                    ex.Message,
                    cancellationToken);
            }
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}