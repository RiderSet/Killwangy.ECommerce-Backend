using StackExchange.Redis;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.SetupExtensions;

public static class InfrastructureSetup
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var redis = configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Redis não configurado.");

            return ConnectionMultiplexer.Connect(redis);
        });

        return services;
    }
}