using Core.Domain.Responses;

namespace AuthorizationService.Domain.Responses
{
    public class LoginResponse : BaseResponse
    {
        /// <summary>
        /// The jwt token cookie
        /// </summary>
        public CookieOptions? Cookie { get; set; }

        /// <summary>
        /// The JWT token
        /// </summary>
        public string Token  { get; set; }
    }
}
