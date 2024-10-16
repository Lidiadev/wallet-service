namespace Wallet.Application.Interfaces;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
}