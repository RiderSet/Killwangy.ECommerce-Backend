using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public class OutboxWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(2000, stoppingToken);
        }
    }
}
