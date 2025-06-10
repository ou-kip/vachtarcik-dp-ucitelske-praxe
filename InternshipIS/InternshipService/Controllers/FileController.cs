using Asp.Versioning;
using Core.Services;
using InternshipService.Controllers.Base;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternshipService.Controllers
{
    [ApiVersion("1.0")]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="mediator"></param>
        public FileController(IMediator mediator, IFileService fileService) : base(mediator)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Downloads the file from the server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("download")]
        [Authorize(Roles = "Admin, Teacher, Student, CompanyRelative")]
        [ProducesResponseType(typeof(GetInternshipResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadFile([FromBody] DownloadFileCommand command, CancellationToken ct)
        {
            var fileResult = await _fileService.GetFileAsync(command.FileName, command.ParentId.ToString(), ct);
            return File(fileResult.Content, fileResult.ContentType);
        }

        /// <summary>
        /// Uploads the file to the server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("task/upload")]
        [Authorize(Roles = "Admin, Student, Teacher, CompanyRelative")]
        [ProducesResponseType(typeof(UploadFileResponse), StatusCodes.Status201Created)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTaskFile([FromForm] UploadTaskFileCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return CreatedAtAction("UploadTaskFile", result);
        }

        /// <summary>
        /// Uploads the solution file to the server
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("solution/upload")]
        [Authorize(Roles = "Admin, Student, Teacher, CompanyRelative")]
        [ProducesResponseType(typeof(UploadFileResponse), StatusCodes.Status201Created)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadSolutionFile([FromForm] UploadSolutionFileCommand command, CancellationToken ct)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            command.SetPerformer(email);

            var result = await Mediator.Send(command, ct);
            return CreatedAtAction("UploadSolutionFile", result);
        }
    }
}
