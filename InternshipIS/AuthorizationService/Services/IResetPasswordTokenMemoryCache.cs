using IResult = Core.Infrastructure.Results.IResult;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The interface for ResetPasswortToken memory cache
    /// </summary>
    public interface IResetPasswordTokenMemoryCache
    {
        /// <summary>
        /// Checks whether the token exists in the cache
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="ct"></param>
        /// <returns>True if the token exists in memory cache</returns>
        Task<bool> CheckTokenAsync(string identifier, CancellationToken ct = default);

        /// <summary>
        /// Saves the token to cache
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> SaveTokenAsync(string identifier, Func<KeyValuePair<string, string>> action, CancellationToken ct = default);

        /// <summary>
        /// Gets the email and token from memory cache by identifier - key
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<KeyValuePair<string, string>> GetByIdentifier(string identifier, CancellationToken ct = default);
    }
}
