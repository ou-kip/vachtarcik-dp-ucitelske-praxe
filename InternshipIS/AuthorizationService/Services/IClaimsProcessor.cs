using Identity.Database.Entities;
using System.Security.Claims;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The Claims processor interface
    /// </summary>
    public interface IClaimsProcessor
    {
        /// <summary>
        /// Adds the claim to the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddClaimAsync(User user, string claimType, string claimValue, CancellationToken ct = default);

        /// <summary>
        /// Removes the claim from the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task RemoveClaim(User user, string claimType, string claimValue, CancellationToken ct);

        /// <summary>
        /// Gets the users claims indentity
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> GetUserClaimsIdentityAsync(User user, CancellationToken ct);
    }
}
