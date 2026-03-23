using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

public class KeyRotationHostedService : BackgroundService
{
    private readonly KeyRotationService _keys;

    public KeyRotationHostedService(KeyRotationService keys)
    {
        _keys = keys;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);

            var newKey = Convert.ToBase64String(
                Guid.NewGuid().ToByteArray());

            _keys.RotateKey(newKey);
        }
    }
}