using Microsoft.Extensions.Caching.Distributed;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public class RedisLockHandle
{
    private readonly IDistributedCache _cache;

    public RedisLockHandle(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<bool> AcquireLockAsync(string key, TimeSpan expiration)
    {
        var existing = await _cache.GetStringAsync(key);

        if (existing != null)
            return false;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(key, "locked", options);

        return true;
    }

    public async Task ReleaseLockAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}