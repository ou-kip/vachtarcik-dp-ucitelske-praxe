using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the collection of tasks
    /// </summary>
    public class GetTasksCommandHandler : BaseCommandHandler<GetTasksCommand, GetTasksResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipTaskRepository"></param>
        /// <param name="logger"></param>
        public GetTasksCommandHandler(IInternshipTaskRepository internshipTaskRepository, ILogger<GetTasksCommandHandler> logger) : base(logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetTasksResponse>> HandleAsync(GetTasksCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetTasksResponse>();

            var tasksResult = await _internshipTaskRepository.GetAllAsQueryableAsync(ct);
            if (tasksResult.HasErrors)
            {
                Logger.LogError(tasksResult.GetErrorMessage());
                throw new ProcessingException(tasksResult.GetErrorMessage());
            }

            try
            {
                var query = tasksResult.Query;
                var tasks = query.Where(x => x.InternshipId.Equals(request.InternshipId)).ToList();

                result.Data.Tasks = tasks.Select(x => new InternshipTaskDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    EndsOn = x.EndsOn,
                    IsCompleted = x.IsCompleted,
                    Name = x.Name,
                    State = x.State,
                }).ToList();

                result.StatusCode = StatusCodes.Status200OK;
                result.Data.StatusCode = StatusCodes.Status200OK;

                return result;

            }
            catch (Exception ex) 
            {
                Logger.LogError(ex.Message);
                
                result.Data.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;

                return result;
            }
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetTasksCommand request, CancellationToken ct)
        {
            if(request.InternshipId == Guid.Empty)
            {
                Logger.LogError("INTERNSHIP TASK: InternshipId value is not present.");
                throw new ArgumentNullException(nameof(request.InternshipId), "InternshipId value must be filled.");
            }

            return Task.CompletedTask;
        }
    }
}
