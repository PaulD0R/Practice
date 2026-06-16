namespace NotificationService.Application.Interfaces.Caching;

public interface ICachingService
{
    Task SetAsync<T>(string key, T value, TimeSpan expiration);
    Task<T?> GetAsync<T>(string key);
}