using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// Command handler for getting the task
    /// </summary>
    public class GetTaskCommandHandler : BaseCommandHandler<GetTaskCommand, GetTaskResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipTaskRepository"></param>
        /// <param name="logger"></param>
        public GetTaskCommandHandler(IInternshipTaskRepository internshipTaskRepository, ILogger<GetTaskCommandHandler> logger) : base(logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetTaskResponse>> HandleAsync(GetTaskCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetTaskResponse>();

            var taskResult = await _internshipTaskRepository.GetWithFilesAsync(request.Id, ct);
            if (taskResult.HasErrors || taskResult.Data == null)
            {
                throw new NotFoundException(typeof(Task).Name, request.Id.ToString());
            }

            var taskDto = new InternshipTaskDto()
            {
                Id = taskResult.Data.Id,
                Description = taskResult.Data.Description,
                CreatedOn = taskResult.Data.CreationDate,
                EndsOn = taskResult.Data.EndsOn,
                IsReported = taskResult.Data.IsReported,
                Links = taskResult.Data.Links?.Select(x => new LinkDto() { Id = x.Id, Name = x.Name, Url = x.Url }).ToList(),
                Name = taskResult.Data.Name,
                State = taskResult.Data.State,
                Summary = taskResult.Data.Summary,
                TeacherSummary = taskResult.Data.TeacherSummary,
                Files = taskResult.Data.Files?.Select(x => new FileDto() { Id = x.Id, FileName = x.FileName }).ToList(),
                InternshipId = taskResult.Data.InternshipId,
            };

            result.Data.Task = taskDto;
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetTaskCommand request, CancellationToken ct)
        {
            if (request.Id == Guid.Empty || request.Id == null)
            {
                throw new ArgumentNullException(nameof(request.Id), "Task Id must be filled in order to get task details");
            }

            return Task.CompletedTask;
        }
    }
}
