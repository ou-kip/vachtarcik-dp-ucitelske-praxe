using Core.Exceptions;
using Identity.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The Claims processor
    /// </summary>
    public class ClaimsProcessor : IClaimsProcessor
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor for ClaimsProcessor
        /// </summary>
        /// <param name="userManager"></param>
        public ClaimsProcessor(UserManager<User> userManager, ILogger<ClaimsProcessor> logger) 
        {
            _userManager = userManager;
            _logger = logger;
        }

        ///<inheritdoc/>
        public async Task AddClaimAsync(User user, string claimType, string claimValue, CancellationToken ct)
        {
            await AddClaimInternalAsync(user, claimType, claimValue, ct);
        }

        ///<inheritdoc/>
        public async Task RemoveClaim(User user, string claimType, string claimValue, CancellationToken ct)
        {
            await RemoveClaimInternal(user, claimType,claimValue, ct);
        }

        ///<inheritdoc/>
        public async Task<ClaimsIdentity> GetUserClaimsIdentityAsync(User user, CancellationToken ct)
        {
            return await GetClaimsIdentityAsync(user, ct);
        }

        /// <summary>
        /// Adds the claim to the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ProcessingException"></exception>
        private async Task AddClaimInternalAsync(User user, string claimType, string claimValue, CancellationToken ct)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if(string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));
            if(string.IsNullOrEmpty(claimValue)) throw new ArgumentNullException(nameof(claimValue));

            var claim = new Claim(claimType, claimValue);

            var result = await _userManager.AddClaimAsync(user, claim);
            if (result.Errors.Any())
            {
                _logger.LogError($"AUTHORIZATION: Unable to assign claim ({claimType}) for user with Id: {user.Id}");
                throw new ProcessingException("AUTHORIZATION: Unable to assign claim for user");
            }
        }

        /// <summary>
        /// Removes the claim from user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ProcessingException"></exception>
        private async Task RemoveClaimInternal(User user, string claimType, string claimValue, CancellationToken ct)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));
            if (string.IsNullOrEmpty(claimValue)) throw new ArgumentNullException(nameof(claimValue));
            
            var claims = await _userManager.GetClaimsAsync(user);
            var claim = claims.FirstOrDefault(x=>x.Type.Equals(claimType) && x.Value.Equals(claimValue));

            var result = await _userManager.RemoveClaimAsync(user, claim);
            if (result.Errors.Any())
            {
                _logger.LogError($"AUTHORIZATION: Unable to remove claim ({claimType}) for user with Id: {user.Id}");
                throw new ProcessingException("AUTHORIZATION: Unable to remove claim from user");
            }
        }

        /// <summary>
        /// Gets the user claims identity
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<ClaimsIdentity> GetClaimsIdentityAsync(User user, CancellationToken ct)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            return new ClaimsIdentity(claims, "Custom"); //TODO
        }
    }
}
