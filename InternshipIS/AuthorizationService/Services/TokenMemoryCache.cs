using Core.Infrastructure.Results;
using Core.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using IResult = Core.Infrastructure.Results.IResult;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The class representing memory cache for tokens
    /// </summary>
    public class TokenMemoryCache : BaseMemoryCache<string>, ITokenMemoryCache
    {
        /// <summary>
        /// .ctor for TokenMemoryCache
        /// </summary>
        /// <param name="memoryCache"></param>
        public TokenMemoryCache(IMemoryCache memoryCache) : base(memoryCache)
        {
        }

        ///<inheritdoc/>
        protected override string CacheKey => "Bearer";

        ///<inheritdoc/>
        public async Task<bool> CheckTokenAsync(string token, CancellationToken ct = default)
        {
            var result = await GetCacheValueAsync(token, ct);

            if (!string.IsNullOrEmpty(result))
            {
                return true;
            }

            return false;
        }

        ///<inheritdoc/>
        public async Task<IResult> SaveTokenAsync(string token, Func<string> action, CancellationToken ct = default)
        {
            var result = new Result();

            try
            {
                _ = await GetOrCreateCacheValueAsync(token, action, 120, ct);
            }
            catch (Exception ex) 
            {
                result.AddError(ex);
            }

            return result;
        }
    }
}
