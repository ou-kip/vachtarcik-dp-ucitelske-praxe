using Asp.Versioning;
using Core.Domain.Responses;
using InternshipService.Controllers.Base;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;

namespace InternshipService.Controllers
{
    [ApiVersion("1.0")]
    public class InternshipController : BaseController
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="mediator"></param>
        public InternshipController(IMediator mediator) : base(mediator)
        {
        }

        #region Internships

        /// <summary>
        /// Gets the internship by its ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetInternshipResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetInternshipCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets the collection of internships based on the provided command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [HttpGet("getcollection")]
        [ProducesResponseType(typeof(GetCollectionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollection([FromQuery] GetCollectionCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new internship with the provided command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Updates the internship with the provided command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok();
        }

        /// <summary>
        /// Deletes the internship by its ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin, Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromQuery] DeleteCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return NoContent();
        }

        /// <summary>
        /// Assigns the internship to the current user (e.g., student)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("assigntome")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        [ProducesResponseType(typeof(AssignToMeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignToMe([FromQuery] AssignToMeCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        #endregion

        #region Tasks

        /// <summary>
        /// Gets the task by its ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("task/get")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(GetTaskResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTask([FromQuery] GetTaskCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets the collection of tasks based on the provided command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("task/getCollection")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(GetTasksResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskCollection([FromQuery] GetTasksCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets the filtered collection of tasks based on the provided command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("task/filter/getCollection")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetFilteredTasksResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskCollectionFiltered([FromQuery] GetFilteredTasksCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Gets the files associated with the specified task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("task/getFiles")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(GetTaskFilesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTaskFiles([FromQuery] GetTaskFilesCommand command, CancellationToken ct)
        {
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new task for the specified internship
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("task/create")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(CreateTasksResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTasksCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return CreatedAtAction("CreateTask", result);
        }

        /// <summary>
        /// Updates the specified task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("task/update")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(UpdateTaskResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        #endregion

        #region Task Solutions

        /// <summary>
        /// Creates a solution for the specified task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("task/solution/create")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(CreateSolutionResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSolution([FromBody] CreateSolutionCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return CreatedAtAction("CreateSolution", result);
        }

        /// <summary>
        /// Gets the solution for the specified task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("task/solution/get")]
        [Authorize(Roles = "Admin, Teacher, CompanyRelative, Student")]
        [ProducesResponseType(typeof(CreateTasksResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> GetSolution([FromQuery] GetSolutionCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        #endregion

        /// <summary>
        /// Gets the list of categories for internships and tasks
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("get/categories")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)] 
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesCommand command, CancellationToken ct)
        {   
            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Checks whether the user has allowed action on specified internship or task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("get/allowedactions")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(AllowedActionsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AllowedActions([FromQuery] AllowedActionsCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return Ok(result);
        }

        /// <summary>
        /// Checks whether the user has allowed action on specified internship or task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("get/export")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Export([FromBody] ExportCommand command, CancellationToken ct)
        {
            var result = (await Mediator.Send(command, ct)).Data;
            return File(result.FileContent, "text/html", result.FileName);
        }
    }
}
