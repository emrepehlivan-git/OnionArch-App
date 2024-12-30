using System.Text;
using System.Text.Json;
using ECommerce.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services;

public sealed class RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger) : ICacheService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    private readonly ILogger<RedisCacheService> _logger = logger;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await distributedCache.GetAsync(key, cancellationToken);
        if (cachedValue is null)
        {
            return default!;
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing cache value for key: {Key}", key);
            return default!;
        }
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
