using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Wallet.Application.Interfaces;

namespace Wallet.Infrastructure.Caching;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
    {
        var cachedValue = await distributedCache.GetStringAsync(key);
        if (cachedValue != null)
        {
            return JsonConvert.DeserializeObject<T>(cachedValue);
        }

        var result = await factory();
        await SetAsync(key, result, expiry);
        return result;
    }

    public async Task RemoveAsync(string key)
    {
        await distributedCache.RemoveAsync(key);
    }

    private async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions();
        if (expiry.HasValue)
        {
            options.SetAbsoluteExpiration(expiry.Value);
        }

        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);
    }
}