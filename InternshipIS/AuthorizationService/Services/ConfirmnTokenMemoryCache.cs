using Core.Infrastructure.Results;
using Core.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using IResult = Core.Infrastructure.Results.IResult;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The memory cache for confirmnation tokens
    /// </summary>
    public class ConfirmnTokenMemoryCache : BaseMemoryCache<KeyValuePair<string, string>>, IConfirmnTokenMemoryCache
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="memoryCache"></param>
        public ConfirmnTokenMemoryCache(IMemoryCache memoryCache) : base(memoryCache)
        {
        }

        /// <inheritdoc/>
        protected override string CacheKey => "ConfirmToken";

        ///<inheritdoc/>
        public async Task<bool> CheckTokenAsync(string identifier, CancellationToken ct = default)
        {
            var result = await GetCacheValueAsync(identifier, ct);

            if (!string.IsNullOrEmpty(result.Key) && !string.IsNullOrEmpty(result.Value))
            {
                return true;
            }

            return false;
        }

        ///<inheritdoc/>
        public async Task<IResult> SaveTokenAsync(string identifier, Func<KeyValuePair<string, string>> action, CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                _ = await GetOrCreateCacheValueAsync(identifier, action, 15, ct);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        ///<inheritdoc/>
        public Task<KeyValuePair<string, string>> GetByIdentifier(string identifier, CancellationToken ct = default)
        {
            return GetCacheValueAsync(identifier);
        }
    }
}
