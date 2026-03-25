using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Api.Filters;

public class IdempotencyFilter : IAsyncActionFilter
{
    private readonly IDistributedCache _cache;

    public IdempotencyFilter(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var key = context.HttpContext.Request.Headers["Idempotency-Key"]
            .FirstOrDefault();

        if (string.IsNullOrEmpty(key))
        {
            await next();
            return;
        }

        var cacheKey = $"idempotency:{key}";

        // 🔎 Consulta Redis
        var existing = await _cache.GetAsync(cacheKey);

        if (existing != null)
        {
            context.Result = new ConflictObjectResult(new
            {
                message = "Duplicate request detected"
            });

            return;
        }

        // executa endpoint
        var executedContext = await next();

        // grava no Redis com TTL
        await _cache.SetAsync(
            cacheKey,
            Encoding.UTF8.GetBytes("processed"),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
    }
}