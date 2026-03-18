using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace GBastos.Casa_dos_Farelos.Observability.Correlations;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdOptions _options;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        CorrelationIdOptions options)
    {
        _next = next;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[_options.RequestHeader]
                .FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;

        if (_options.IncludeInResponse)
            context.Response.Headers[_options.RequestHeader] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}