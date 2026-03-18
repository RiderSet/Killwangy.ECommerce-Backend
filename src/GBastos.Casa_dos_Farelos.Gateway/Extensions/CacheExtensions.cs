using StackExchange.Redis;

namespace GBastos.Casa_dos_Farelos.Gateway.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddGatewayCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connection = configuration["Redis:Connection"];

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connection;
        });

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(connection!));

        return services;
    }
}