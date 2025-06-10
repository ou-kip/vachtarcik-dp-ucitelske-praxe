using IResult = Core.Infrastructure.Results.IResult;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The interface for TokenMemoryCache
    /// </summary>
    public interface ITokenMemoryCache
    {
        /// <summary>
        /// Checks whether the token exists in the cache
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns>True if the token exists in memory cache</returns>
        Task<bool> CheckTokenAsync(string token, CancellationToken ct = default);

        /// <summary>
        /// Saves the token to cache
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> SaveTokenAsync(string token, Func<string> action, CancellationToken ct = default);
    }
}
