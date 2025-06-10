using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    public class GetTaskSolutionCommandHandler : BaseInternshipCommandHandler<GetSolutionCommand, GetSolutionResponse>
    {
        private readonly IInternshipTaskSolutionRepository _repository;
        private readonly IInternshipTaskRepository _taskRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="taskRepository"></param>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="logger"></param>
        public GetTaskSolutionCommandHandler(
            IInternshipTaskSolutionRepository repository,
            IInternshipTaskRepository taskRepository,
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            IInternshipCategoryRepository internshipCategoryRepository, 
            IInternshipLinkRepository internshipLinkRepository, 
            ILogger<GetTaskSolutionCommandHandler> logger) 
            : base(
                  internshipRepository, 
                  internshipTeacherRepository, 
                  internshipStudentRepository, 
                  internshipCompanyRelativeRepository, 
                  internshipCategoryRepository, 
                  internshipLinkRepository, 
                  logger)
        {
            _repository = repository;
            _taskRepository = taskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetSolutionResponse>> HandleAsync(GetSolutionCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetSolutionResponse>();
            var solutionResult = await _repository.GetWithFilesByTaskIdAsync(request.TaskId, ct);

            if(solutionResult.HasErrors || solutionResult.Data == null)
            {
                throw new ProcessingException($"Unable to get solution for task with ID: {request.TaskId}");
            }

            result.Data.Solution = new SolutionDto
            {
                Id = solutionResult.Data.Id,
                Author = solutionResult.Data.CreationAuthor,
                Solution = solutionResult.Data.Solution,
                SubmittedDate = solutionResult.Data.SubmittedDate,
                Files = solutionResult.Data.Files.Select(x => new FileDto() { Id = x.Id, FileName = x.FileName}).ToList()
            };

            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override async Task ValidateDataAsync(GetSolutionCommand request, CancellationToken ct)
        {
            if(request.TaskId == null || request.TaskId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(request.TaskId), "Task ID value must be provided!");
            }

            var task = await _taskRepository.GetAsync(request.TaskId);
            if (task.HasErrors || task.Data == null)
            {
                throw new NotFoundException(nameof(InternshipTask), request.TaskId.ToString());
            }
        }
    }
}
