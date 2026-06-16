using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Application.Interfaces.Caching;

namespace NotificationService.Infrastructure.Redis;

public class RedisService(IDistributedCache cache) : ICachingService
{
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await cache.GetAsync(key);
        return value != null ? JsonSerializer.Deserialize<T>(value) : default;
    }


}