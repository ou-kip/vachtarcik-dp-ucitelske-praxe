using Microsoft.Extensions.Caching.Memory;

namespace Core.MemoryCache
{
    /// <summary>
    /// The class representing object woking with semaphore and memory cache
    /// </summary>
    public abstract class BaseMemoryCache<T>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly SemaphoreSlim _semaphore;
        private readonly HashSet<string> _cacheKeys;

        /// <summary>
        /// .ctor for BaseMemoryCache
        /// </summary>
        /// <param name="memoryCache"></param>
        public BaseMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _semaphore = new SemaphoreSlim(1, 1);
            _cacheKeys = new HashSet<string>();
        }

        /// <summary>
        /// The cache key
        /// </summary>
        protected abstract string CacheKey { get; }

        /// <summary>
        /// Gets or creates a cache value item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<T> GetOrCreateCacheValueAsync(string key, Func<T> valueFactory, int expirationInMinutes = 5, CancellationToken ct = default)
        {
            var keyedValue = $"{CacheKey}_{key}";

            await _semaphore.WaitAsync(ct);
            try
            {
                if (_memoryCache.TryGetValue<T>(keyedValue, out T value))
                {
                    return value;
                }

                _memoryCache.Set(keyedValue, valueFactory(), new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationInMinutes)});
                _cacheKeys.Add($"{CacheKey}_{key}");

                return value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Gets the value from cache without refreshing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<T> GetCacheValueAsync(string key, CancellationToken ct = default)
        {
            var keyedValue = $"{CacheKey}_{key}";

            await _semaphore.WaitAsync(ct);
            try
            {
                var value = _memoryCache.Get(keyedValue);
                return (T)value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Deletes the value from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task DeleteCacheValueAsync(string key, CancellationToken ct = default)
        {
            var keyedValue = $"{CacheKey}_{key}";

            await _semaphore.WaitAsync(ct);
            try
            {
                _memoryCache.Remove(keyedValue);
                _cacheKeys.Remove($"{CacheKey}_{key}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Deletes all values from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task DeleteAllCacheValuesAsync(CancellationToken ct = default)
        {
            await _semaphore.WaitAsync(ct);
            try
            {
                foreach (var key in _cacheKeys)
                {
                    _memoryCache.Remove(key);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
