using Polly;
using Polly.Extensions.Http;

namespace GBastos.Casa_dos_Farelos.Gateway.Extensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddGatewayResilience(
        this IServiceCollection services)
    {
        services.AddHttpClient("gateway")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreaker());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retry =>
                TimeSpan.FromMilliseconds(200 * retry));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreaker()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}