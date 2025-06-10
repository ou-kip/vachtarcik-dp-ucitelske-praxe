using Core.Domain.Responses;
using Core.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers.Base
{
    /// <summary>
    /// The wrapper with mediator patter for ControllerBase
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Creates instance of Controller with injected mediator
        /// </summary>
        /// <param name="mediator"></param>
        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// The Mediator
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// The response of endpoints
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiDataResult"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected IActionResult ApiResponse<T>(IApiResult<T> apiDataResult) where T : IBaseResponse
        {
            if (apiDataResult == null)
                throw new ArgumentNullException(nameof(apiDataResult));

            if (!apiDataResult.HasErrors)
            {
                return Ok(apiDataResult.Data);
            }
            else if (apiDataResult.HasErrors) 
            {
                return BadRequest(apiDataResult.GetErrorMessage());
            }
            
            //the rest handled by exception middleware

            return Problem(apiDataResult.GetErrorMessage(), null, apiDataResult.StatusCode);
        }

        /// <summary>
        /// Appends the cookie to the response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        protected Task AppendCookie(string name, string value, CookieOptions options)
        {
            Response.Cookies.Append(name, value, options);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Appends the cookie holding the bearer token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        protected Task AppendBearerTokenCookie(string token, CookieOptions options)
        {
            Response.Cookies.Append("Bearer", token, options);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the token from cookie
        /// </summary>
        /// <param name="token"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected string? GetBearerToken()
        {
            if (Request.Cookies.TryGetValue("Bearer", out var token))
            {
                return token;
            }
            else return null;
        }
    }
}
