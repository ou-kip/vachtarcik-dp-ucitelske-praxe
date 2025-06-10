using AuthorizationService.Configuration;
using Contracts;
using Core.Dto;
using Core.Exceptions;
using Core.Helpers;
using Core.Infrastructure.Results;
using Identity.Configuration;
using Identity.Database.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IResult = Core.Infrastructure.Results.IResult;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AuthorizationService.Services
{
    /// <summary>
    /// The Authorization service
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPublishEndpoint _publisher;
        private readonly JwtOptions _options;
        private readonly AuthorizationOptions _authorizationOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenMemoryCache _tokenMemoryCache;
        private readonly ICookieProcessor _cookieProcessor;
        private readonly IResetPasswordTokenMemoryCache _resetPasswordTokenMemoryCache;
        private readonly IConfirmnTokenMemoryCache _confirmnTokenMemoryCache;


        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="publisher"></param>
        /// <param name="tokenMemoryCache"></param>
        /// <param name="cookieProcessor"></param>
        /// <param name="resetPasswordTokenMemoryCache"></param>
        /// <param name="options"></param>
        /// <param name="authOptions"></param>
        /// <param name="confirmnTokenMemoryCache"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthorizationService(
            ILogger<AuthorizationService> logger,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IPublishEndpoint publisher,
            ITokenMemoryCache tokenMemoryCache,
            ICookieProcessor cookieProcessor,
            IResetPasswordTokenMemoryCache resetPasswordTokenMemoryCache,
            IOptions<JwtOptions> options,
            IOptions<AuthorizationOptions> authOptions,
            IConfirmnTokenMemoryCache confirmnTokenMemoryCache)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _tokenMemoryCache = tokenMemoryCache ?? throw new ArgumentNullException(nameof(tokenMemoryCache));
            _cookieProcessor = cookieProcessor ?? throw new ArgumentNullException(nameof(cookieProcessor));
            _resetPasswordTokenMemoryCache = resetPasswordTokenMemoryCache ?? throw new ArgumentNullException(nameof(resetPasswordTokenMemoryCache));
            _authorizationOptions = authOptions.Value ?? throw new ArgumentNullException(nameof(authOptions));
            _confirmnTokenMemoryCache = confirmnTokenMemoryCache ?? throw new ArgumentNullException(nameof(confirmnTokenMemoryCache));
        }

        ///<inheritdoc/>
        public async Task<string> AuthorizeUsingJwtAsync(LoginDto userLogin, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(userLogin.Token) && !await _tokenMemoryCache.CheckTokenAsync(userLogin.Token, ct))
            {
                return string.Empty;
            }

            var token = await GenerateTokenAsync(userLogin, ct);
            await _tokenMemoryCache.SaveTokenAsync(token, () => token);

            return token;
        }

        ///<inheritdoc/>
        public async Task<IDataResult<Guid>> RegisterUserAsync(User user, string password, string roleCode, CancellationToken ct)
        {
            var result = new DataResult<Guid>();

            if (!await _roleManager.RoleExistsAsync(roleCode))
                throw new NullReferenceException($"AUTHORIZATION: Invalid role code, the role '{roleCode}' can´t be found.");

            var registrationResult = await _userManager.CreateAsync(user, password);
            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleCode);

            if (!registrationResult.Succeeded)
            {
                var subject = "AUTHORIZATION: Unable to register user.";
                var msg = $"AUTHORIZATION: Error durign registration user: {user.Email}";
                await _publisher.Publish(new EmailNotification { Receivers = new() { _authorizationOptions.AdministratorEmail }, Subject = subject, Message = msg }, ct);

                _logger.LogError(msg);
                throw new ProcessingException(msg);
            }
            else if (!addToRoleResult.Succeeded)
            {
                var subject = $"AUTHORIZATION: Unable to add user to the role";
                var msg = $"AUTHORIZATION: Error durign registration user, unable to add role: {user.Email}, {roleCode}";
                
                _logger.LogError(msg);

                var deleteResult = await TryDeleteUserAsync(user, ct);
                if (deleteResult.HasErrors)
                {
                    subject = "AUTHORIZATION: Unable to delete user.";
                    msg = $"AUTHORIZATION: Unable to delete user {user.Email} without role";

                    _logger.LogError(msg);
                }

                await _publisher.Publish(new EmailNotification { Receivers = new() { _authorizationOptions.AdministratorEmail }, Subject = subject, Message = msg }, ct);
                throw new ProcessingException(msg);
            }

            if (registrationResult.Succeeded && addToRoleResult.Succeeded)
            {
                await GenerateConfirmnationTokenAsync(user, ct);

                var registeredUserId = await _userManager.GetUserIdAsync(user);
                result.Data = Guid.Parse(registeredUserId);

                await _publisher.Publish(new UserRegisteredNotification { Email = user.Email }, ct);
                await PublishInternshipUserAsync(Guid.Parse(registeredUserId), user, roleCode, ct);
            }

            return result;
        }

        ///<inheritdoc/>
        public async Task<bool> DeleteUserAsync(string userEmail, CancellationToken ct = default)
        {
            var result = new Result();

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                _logger.LogError($"AUTHORIZATION: Unable to find and delete user with email: {userEmail}.");
                return false;
            }

            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var deleteResult = await TryDeleteUserAsync(user, ct);
            if (deleteResult.HasErrors)
            {
                var subject = "AUTHORIZATION: Unable to delete user.";
                var msg = $"AUTHORIZATION: Unable to delete user {user.Email} without role";

                await _publisher.Publish(new EmailNotification { Receivers = new() { _authorizationOptions.AdministratorEmail }, Subject = subject, Message = msg }, ct);
                _logger.LogError(msg);

                return false;
            }

            await _publisher.Publish(new InternshipPersonDeleteNotification() { Email = userEmail, PersonType = ResolveRoleToPersonType(userRole)}, ct);

            return true;
        }

        ///<inheritdoc/>
        public Task<CookieOptions> CreateCookieOptionsAsync(DateTime expires, string? domain = null, CancellationToken ct = default)
        {
            var options = _cookieProcessor.CreateCookie(expires, domain);
            return Task.FromResult(options);
        }

        ///<inheritdoc/>
        public Task<CookieOptions> CreateLogoutCookieOptionsAsync(string? domain = null, CancellationToken ct = default)
        {
            var options = _cookieProcessor.RemoveCookie();
            return Task.FromResult(options);
        }

        ///<inheritdoc/>
        public async Task<string> GenerateResetPasswordTokenAsync(string email, CancellationToken ct = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null) { throw new ProcessingException($"AUTHORIZATION: User with email: {email} does not exist"); }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var identifier = GuidHelper.GenerateShortGuid();

            await _resetPasswordTokenMemoryCache.SaveTokenAsync(identifier, () => { return KeyValuePair.Create(token, email); }, ct);

            await _publisher.Publish(new ResetPasswordNotification { Email = email, Identifier = identifier }, ct);

            return identifier;
        }

        ///<inheritdoc/>
        public async Task<IResult> UpdatePasswordAsync(string identifier, string password, CancellationToken ct = default)
        {
            var isTokenValid = await _resetPasswordTokenMemoryCache.CheckTokenAsync(identifier, ct);
            if (!isTokenValid)
            {
                throw new InvalidTokenException(identifier);
            }

            var emailTokenKvp = await _resetPasswordTokenMemoryCache.GetByIdentifier(identifier);
            if (string.IsNullOrEmpty(emailTokenKvp.Value))
            {
                throw new InvalidTokenException(identifier);
            }

            var email = emailTokenKvp.Value;
            var token = emailTokenKvp.Key;

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null) { throw new InvalidOperationException("User with provided email does not exist!"); }

            var result = await _userManager.ResetPasswordAsync(existingUser, token, password);
            if (result.Succeeded)
            {
                return new Result();
            }
            else
            {
                throw new ProcessingException(result.Errors.Select(x => x.Description).ToList());
            }
        }

        ///<inheritdoc/>
        public async Task<IResult> ConfirmUserEmail(string identifier, CancellationToken ct = default)
        {
            var isTokenValid = await _confirmnTokenMemoryCache.CheckTokenAsync(identifier, ct);
            if (!isTokenValid)
            {
                throw new InvalidTokenException(identifier);
            }

            var emailTokenKvp = await _confirmnTokenMemoryCache.GetByIdentifier(identifier);
            if (string.IsNullOrEmpty(emailTokenKvp.Value))
            {
                throw new InvalidTokenException(identifier);
            }

            var email = emailTokenKvp.Value;
            var token = emailTokenKvp.Key;

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null) { throw new InvalidOperationException("User with provided email does not exist!"); }

            var result = await _userManager.ConfirmEmailAsync(existingUser, token);
            if (result.Succeeded)
            {
                return new Result();
            }
            else
            {
                throw new ProcessingException(result.Errors.Select(x => x.Description).ToList());
            }
        }

        /// <summary>
        /// Registers the jwt token
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<string> GenerateTokenAsync(LoginDto userLogin, CancellationToken ct)
        {
            _ = await TrySignInAsync(userLogin, ct);

            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.GivenName, user.LastName),
                new Claim(ClaimTypes.Role, role),
            };

            userClaims.ToList().ForEach(x => claims.Append(x));

            var claimsIdentity = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var signUp = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null, DateTime.UtcNow.AddHours(3), signUp);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Try to sign in user or throw exception
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="SignInException"></exception>
        private async Task<SignInResult> TrySignInAsync(LoginDto userLogin, CancellationToken ct)
        {
            var result = new SignInResult();
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user != null && user.EmailConfirmed)
            {
                result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, false);
                if (!result.Succeeded)
                {
                    throw new SignInException(userLogin.Email);
                }
                else if (result.IsLockedOut)
                {
                    throw new SignInException("AUTHORIZATION: The account is locked!", userLogin.Email);
                }
                else if (result.IsNotAllowed)
                {
                    throw new SignInException("AUTHORIZATION: The email is not confirmed.", userLogin.Email);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes the user from db
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<IResult> TryDeleteUserAsync(User user, CancellationToken ct)
        {
            var result = new Result();

            var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                return result;
            }
            else
            {
                result.AddError(new Exception($"AUTHORIZATION: Unable to delete user withour role: {user.Email} "));
                return result;
            }
        }

        /// <summary>
        /// Publish the message when user is created to create entity representing user in internship
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        private async Task PublishInternshipUserAsync(Guid userId, User user, string roleCode, CancellationToken ct)
        {
            var notification = new InternshipPersonCreateNotification()
            {
                UserId = userId,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                CompanyName = user.CompanyName,
            };

            switch (roleCode)
            {
                case "Student":
                    {
                        notification.StudentCode = user.Code ?? "";
                        notification.PersonType = InternshipPersonTypeEnum.Student;
                        await _publisher.Publish(notification, ct);

                        break;
                    }
                case "Teacher":
                    {
                        notification.PersonType = InternshipPersonTypeEnum.Teacher;
                        await _publisher.Publish(notification, ct);

                        break;
                    }
                case "CompanyRelative":
                    {
                        notification.PersonType = InternshipPersonTypeEnum.CompanyRelative;
                        await _publisher.Publish(notification, ct);

                        break;
                    }
            }
        }

        /// <summary>
        /// Generates the confirmation token for the user and sends it via email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ProcessingException"></exception>
        private async Task GenerateConfirmnationTokenAsync(User user, CancellationToken ct)
        {
            if(user == null) 
            { 
                throw new ArgumentNullException(nameof(user)); 
            }

            var confirmnationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (string.IsNullOrEmpty(confirmnationToken))
            {
                throw new ProcessingException($"AUTHORIZATION: Unable to generate confirmation token for user {user.Email}");
            }

            var identifier = GuidHelper.GenerateShortGuid();
            await _confirmnTokenMemoryCache.SaveTokenAsync(identifier, () => { return KeyValuePair.Create(confirmnationToken, user.Email); }, ct);

            await _publisher.Publish(new ConfirmnAccountNotification { Email = user.Email, Identifier = identifier }, ct);
        }

        /// <summary>
        /// Resolves and maps role code to the person type
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static InternshipPersonTypeEnum ResolveRoleToPersonType(string roleCode) 
        {
            return roleCode switch
            {
                "Student" => InternshipPersonTypeEnum.Student,
                "Teacher" => InternshipPersonTypeEnum.Teacher,
                "CompanyRelative" => InternshipPersonTypeEnum.CompanyRelative,
                _ => throw new ArgumentException($"Invalid role code: {roleCode}")
            };
        }
    }
}
