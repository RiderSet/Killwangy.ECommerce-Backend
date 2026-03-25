using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Behavior.Idenpotency;

public class RedisIdempotencyStore : IIdempotencyStore
{
    private readonly IDistributedCache _cache;

    public RedisIdempotencyStore(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var value = await _cache.GetAsync(key);
        return value != null;
    }

    public async Task SetAsync(string key)
    {
        await _cache.SetAsync(
            key,
            Encoding.UTF8.GetBytes("processed"),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(10)
            });
    }
}