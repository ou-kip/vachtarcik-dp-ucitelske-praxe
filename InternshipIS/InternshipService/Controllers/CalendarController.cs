using Asp.Versioning;
using InternshipService.Controllers.Base;
using InternshipService.Domain;
using InternshipService.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternshipService.Controllers
{
    [ApiVersion("1.0")]
    public class CalendarController : BaseController
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="mediator"></param>
        public CalendarController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets a collection of teachers
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetCalendarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetCalendarResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GetCalendarResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] GetCalendarCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }
    }
}
