using GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    msg.Lock(lockId, TimeSpan.FromSeconds(30));

                    await publisher.PublishAsync(
                        queueName: msg.Type,
                        message: msg.Payload,
                        messageId: msg.Id,
                        eventType: msg.Type,
                        occurredOnUtc: msg.OccurredOnUtc,
                        version: 1,
                        ct: stoppingToken);

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