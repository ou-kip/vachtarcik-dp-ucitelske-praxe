namespace AuthorizationService.Services
{
    /// <summary>
    /// The cookie processor
    /// </summary>
    public class CookieProcessor : ICookieProcessor
    {
        IWebHostEnvironment _environment;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public CookieProcessor(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        ///<inheritdoc/>
        public CookieOptions CreateCookie(DateTime expires, string? domain = null)
        {
            if (expires == default(DateTime)) { throw new ArgumentNullException(nameof(expires)); }

            var options = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                MaxAge = TimeSpan.FromSeconds((expires - DateTime.Now).TotalSeconds)
            };

            if (!string.IsNullOrEmpty(domain))
            {
                options.Domain = domain;
            }

            return options;

        }

        ///<inheritdoc/>
        public CookieOptions RemoveCookie(string? domain = null)
        {
            var options = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(-1),
                MaxAge = TimeSpan.FromSeconds(0)
            };

            if (!string.IsNullOrEmpty(domain))
            {
                options.Domain = domain;
            }

            return options;

        }
    }
}