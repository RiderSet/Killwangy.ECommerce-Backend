using Microsoft.AspNetCore.RateLimiting;

namespace GBastos.Casa_dos_Farelos.Gateway.Extensions;

public static class RateLimitExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("gateway", config =>
            {
                config.PermitLimit = 200;
                config.Window = TimeSpan.FromSeconds(10);
                config.QueueLimit = 10;
            });
        });

        return services;
    }
}