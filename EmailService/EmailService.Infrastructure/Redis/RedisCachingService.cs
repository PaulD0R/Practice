using System.Text.Json;
using EmailService.Application.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace EmailService.Infrastructure.Redis;

public class RedisCachingService(IDistributedCache cache) : ICachingService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var stringResult = await cache.GetStringAsync(key);
        return stringResult == null ? default : JsonSerializer.Deserialize<T>(stringResult);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), cacheOptions);
    }
}