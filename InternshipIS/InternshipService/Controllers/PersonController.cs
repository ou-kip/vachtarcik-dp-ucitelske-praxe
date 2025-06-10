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
    public class PersonController : BaseController
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="mediator"></param>
        public PersonController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets a collection of teachers
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("getteachers")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetTeachersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeachers([FromQuery] GetTeachersCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets company relative persons
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("getrelatives")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetCompanyRelativesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelatives([FromQuery] GetCompanyRelativesCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets the students
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("getstudents")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetStudentsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudents([FromQuery] GetStudentsCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets all persons
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetAllPersonsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPersonsCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }
    }
}
