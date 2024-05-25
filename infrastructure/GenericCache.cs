using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace backend.Helpers;

public sealed class GenericCache<T> : IDisposable
{
    private readonly IMemoryCache cache;
    public readonly MemoryCacheEntryOptions cacheOptions;
    private bool _disposed;
    private readonly IConfiguration config;

    public GenericCache(IMemoryCache? cache, IConfiguration? config)
    {
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        this.config = config ?? throw new ArgumentNullException(nameof(config));

        if (!int.TryParse(this.config["cache_expiry_minutes"], out var expiry))
        {
            throw new FormatException($"The value of 'cache_expiry_minutes' in the configuration is not a valid integer: {this.config["cache_expiry_minutes"]}");
        }

        cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(expiry));
    }

    public async Task<T?> GetOrCreate(object key, Func<Task<T>> createItemAsync)
    {
        if (!cache.TryGetValue(key, out T? foundItem))
        {
            foundItem = await createItemAsync();
            cache.Set(key, foundItem, cacheOptions);
        }

        return foundItem;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                cache.Dispose();
            }

            _disposed = true;
        }
    }
}

public interface IGenericCache<T>
{
    T? GetOrCreate(object key, Func<T> createItem);
}