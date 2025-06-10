using Core.Dto;
using Core.Infrastructure.Results;
using Identity.Database.Entities;
using IResult = Core.Infrastructure.Results.IResult;

namespace AuthorizationService.Services
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// authorizes the users and provides JWT token
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<string> AuthorizeUsingJwtAsync(LoginDto userLogin, CancellationToken ct);

        /// <summary>
        /// Registers the users account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="roleCode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IDataResult<Guid>> RegisterUserAsync(User user, string password, string roleCode, CancellationToken ct = default);

        /// <summary>
        /// Deletes the users account
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<bool> DeleteUserAsync(string userEmail, CancellationToken ct = default);

        /// <summary>
        /// Creates a new cookie options
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public Task<CookieOptions> CreateCookieOptionsAsync(DateTime expires, string? domain = null, CancellationToken ct = default);

        /// <summary>
        /// Creates a cookie options which will remove the cookie from browser
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public Task<CookieOptions> CreateLogoutCookieOptionsAsync(string? domain = null, CancellationToken ct = default);

        /// <summary>
        /// Resets the passwords and ge
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<string> GenerateResetPasswordTokenAsync(string email, CancellationToken ct = default);

        /// <summary>
        /// Updates the password
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IResult> UpdatePasswordAsync(string identifier, string password, CancellationToken ct = default);

        /// <summary>
        /// Confirmes the user email by identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IResult> ConfirmUserEmail(string identifier, CancellationToken ct = default);
    }
}
