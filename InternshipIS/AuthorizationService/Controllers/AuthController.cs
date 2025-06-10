using Asp.Versioning;
using AuthorizationService.Controllers.Base;
using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Enums;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthorizationService.Controllers
{
    [ApiVersion("1.0")]
    public class AuthController : BaseController
    {
        private readonly ITokenMemoryCache _tokenMemoryCache;

        /// <summary>
        /// .ctor for AuthController
        /// </summary>
        /// <param name="mediator"></param>
        public AuthController(IMediator mediator, ITokenMemoryCache tokenMemoryCache) : base(mediator)
        {
            _tokenMemoryCache = tokenMemoryCache ?? throw new ArgumentNullException(nameof(tokenMemoryCache));
        }

        /// <summary>
        /// Register the student
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("register/student")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            command.SetRole(RoleEnum.Student.ToString());
            var result = await Mediator.Send(command, ct);

            if (!result.HasErrors)
            {
                return StatusCode(result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode(result.StatusCode, result.GetErrorMessage());
            }
        }

        /// <summary>
        /// Register person relative to company
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("register/relative")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterCompanyRelative([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            command.SetRole(RoleEnum.CompanyRelative.ToString());
            var result = await Mediator.Send(command);

            if (!result.HasErrors)
            {
                return StatusCode(result.StatusCode, result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Register a teacher
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("register/teacher")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            command.SetRole(RoleEnum.Teacher.ToString());
            var result = await Mediator.Send(command);

            if (!result.HasErrors)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Confirmes the registration of the user by token
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("register/confirmn")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ConfirmRegistrationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConfirmRegistrationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ConfirmRegistrationResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ConfirmRegistrationResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationCommand command, CancellationToken ct)
        {
            _ = await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Log in the user
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            if(Request.Cookies.Any())
            {
                foreach (var cookie in Request.Cookies)
                {
                    Response
                        .Cookies.Append(
                            cookie.Key,
                            string.Empty,
                            new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddMonths(-1),
                                Path = "/",
                                HttpOnly = true,
                                Secure = true
                            });
                }
            }

            command.SetToken(GetBearerToken());
            var result = await Mediator.Send(command);

            await AppendBearerTokenCookie(result.Data.Token, result.Data.Cookie);
            return Ok();
        }

        /// <summary>
        /// Logouts the user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            if (Request.Cookies.TryGetValue("Bearer", out var token))
            {
                if (!await _tokenMemoryCache.CheckTokenAsync(token, ct))
                {
                    return Ok();
                }
            }

            foreach (var cookie in Request.Cookies)
            {
                Response
                    .Cookies.Append(
                        cookie.Key,
                        string.Empty,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddMonths(-1),
                            Path = "/",
                            HttpOnly = true,
                            Secure = true
                        });
            }

            return Ok();
        }

        /// <summary>
        /// Checks the token from client
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("login/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CheckLogin(CancellationToken ct)
        {
            if (Request.Cookies.TryGetValue("Bearer", out var token))
            {
                if (!await _tokenMemoryCache.CheckTokenAsync(token, ct))
                {
                    return Unauthorized();
                }

                return Ok();
            }

            return Unauthorized();
        }

        /// <summary>
        /// Deletes the user by email
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(DeleteUserCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Provides the role from token
        /// </summary>
        /// <returns></returns>
        [HttpGet("role/getuser")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult GetUser()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var firstName = User.FindFirst(ClaimTypes.Name)?.Value;
            var lastName = User.FindFirst(ClaimTypes.GivenName)?.Value;

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return Unauthorized(new GetUserResponse());
            }

            if (firstName == null || lastName == null)
            {
                throw new ProcessingException("Unable to retrieve all user data.");
            }

            return Ok(new GetUserResponse(userRole, string.Join(" ", firstName, lastName)) { StatusCode = StatusCodes.Status200OK });
        }

        /// <summary>
        /// Generates the token for resetting the password    
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("password/reset")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenerateResetPasswordTokenResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateResetPasswordToken([FromBody] GenerateResetPasswordTokenCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Updates the password of specific user
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("password/update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }
    }
}
