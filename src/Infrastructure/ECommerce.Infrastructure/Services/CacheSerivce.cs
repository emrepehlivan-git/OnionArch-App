using System.Text;
using System.Text.Json;
using ECommerce.Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Infrastructure.Services;

public sealed class CacheSerivce(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await distributedCache.GetAsync(key, cancellationToken);
        return cachedValue is null ? default! : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expirationTime, CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        byte[] byteArray = Encoding.UTF8.GetBytes(serializedValue);
        await distributedCache.SetAsync(key, byteArray, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.Add(expirationTime),
            SlidingExpiration = expirationTime / 2
        }, cancellationToken);
    }
}
