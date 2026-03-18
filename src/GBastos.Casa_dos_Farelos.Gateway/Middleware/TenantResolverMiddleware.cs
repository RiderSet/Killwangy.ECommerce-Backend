namespace GBastos.Casa_dos_Farelos.Gateway.Middleware;

public class TenantResolverMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var tenantId = context.Request.Headers["X-Tenant-ID"];

        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Items["TenantId"] = tenantId.ToString();
        }

        await _next(context);
    }
}