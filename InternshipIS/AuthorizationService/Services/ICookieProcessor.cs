namespace AuthorizationService.Services
{
    /// <summary>
    /// The interface for CookieProcessor
    /// </summary>
    public interface ICookieProcessor
    {
        /// <summary>
        /// Creates a cookie options
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        CookieOptions CreateCookie(DateTime expires, string? domain = null);

        /// <summary>
        /// Updates the cookie expiry date, so it is removed from browser
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        CookieOptions RemoveCookie(string? domain = null);
    }
}
