using Core.Domain.Responses;
using Core.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers.Base
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
    }
}
