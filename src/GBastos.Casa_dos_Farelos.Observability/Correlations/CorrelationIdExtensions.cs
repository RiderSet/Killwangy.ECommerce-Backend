using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Observability.Correlations;

public static class CorrelationIdExtensions
{
    public static IServiceCollection AddDefaultCorrelationId(
        this IServiceCollection services,
        Action<CorrelationIdOptions>? configure = null)
    {
        if (configure != null)
        {
            var options = new CorrelationIdOptions();
            configure(options);

            services.AddSingleton(options);
        }
        else
        {
            services.AddSingleton(new CorrelationIdOptions());
        }

        return services;
    }

    public static IApplicationBuilder UseCorrelationId(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}