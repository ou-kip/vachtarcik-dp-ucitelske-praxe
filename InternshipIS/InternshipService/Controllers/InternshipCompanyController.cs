using Asp.Versioning;
using InternshipService.Controllers.Base;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipService.Controllers
{
    [ApiVersion("1.0")]
    public class InternshipCompanyController : BaseController
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="mediator"></param>
        public InternshipCompanyController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets a collection of companies
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("company/getcollection")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCompaniesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyCollection([FromQuery] GetCompaniesCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }
    }
}
