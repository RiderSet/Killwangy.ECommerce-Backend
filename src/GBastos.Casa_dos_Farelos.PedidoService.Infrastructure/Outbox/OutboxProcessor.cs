using GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;
using GBastos.Casa_dos_Farelos.PedidoService.Infrastructure.Persistence.Context;
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

            var db = scope.ServiceProvider.GetRequiredService<PedidoDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    await publisher.PublishAsync(
                        queueName: msg.Type,
                        message: msg.Content,
                        messageId: msg.Id,
                        eventType: msg.Type,
                        occurredOnUtc: msg.OccurredOnUtc,
                        version: 1,
                        ct: stoppingToken);

                    msg.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkAsFailed(ex.Message);
                }
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(
                TimeSpan.FromSeconds(2),
                stoppingToken);
        }
    }
}