using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Behavior;

public class IdempotencyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDistributedCache _cache;

    public IdempotencyBehavior(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var idempotencyKey = GetIdempotencyKey(request);

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return await next();

        var cacheKey = $"idem:{typeof(TRequest).Name}:{idempotencyKey}";

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cached is not null)
        {
            var response = JsonSerializer.Deserialize<TResponse>(cached);
            if (response is not null)
                return response;
        }

        var result = await next();

        var serialized = JsonSerializer.Serialize(result);

        await _cache.SetStringAsync(
            cacheKey,
            serialized,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        return result;
    }

    private static string? GetIdempotencyKey(TRequest request)
    {
        var prop = typeof(TRequest).GetProperty("IdempotencyKey");
        return prop?.GetValue(request)?.ToString();
    }
}