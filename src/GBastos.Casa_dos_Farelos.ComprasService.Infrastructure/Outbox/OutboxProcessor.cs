using GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.RabbitMQ;
using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _sp;

    public OutboxProcessor(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var lockId = Guid.NewGuid();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null &&
                            (x.LockedUntilUtc == null ||
                             x.LockedUntilUtc < DateTime.UtcNow))
                .OrderBy(x => x.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    var payload = JsonSerializer.Deserialize<object>(msg.Payload);

                    msg.Lock(lockId, TimeSpan.FromSeconds(30));

                    await publisher.PublishAsync(
                        routingKey: msg.Type,
                        payload: msg.Payload,
                        messageId: msg.Id,
                        eventType: msg.Type,
                        occurredOnUtc: msg.OccurredOnUtc,
                        version: 1,
                        cancellationToken: stoppingToken);

                    msg.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkFailed(ex.Message);
                }
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }
}