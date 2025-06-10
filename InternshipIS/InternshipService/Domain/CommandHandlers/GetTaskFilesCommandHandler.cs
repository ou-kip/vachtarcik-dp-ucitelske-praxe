using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the task files
    /// </summary>
    public class GetTaskFilesCommandHandler : BaseCommandHandler<GetTaskFilesCommand, GetTaskFilesResponse>
    {
        private readonly IInternshipTaskFileRepository _internshipTaskFileRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipTaskFileRepository"></param>
        /// <param name="logger"></param>
        public GetTaskFilesCommandHandler(IInternshipTaskFileRepository internshipTaskFileRepository, ILogger<GetTaskFilesCommandHandler> logger) : base(logger)
        {
            _internshipTaskFileRepository = internshipTaskFileRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetTaskFilesResponse>> HandleAsync(GetTaskFilesCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetTaskFilesResponse>();

            var filesResult = await _internshipTaskFileRepository.GetByTaskIdAsync(request.TaskId, ct);
            if (filesResult.HasErrors)
            {
                throw new ProcessingException(filesResult.GetErrorMessage());
            }
            if (filesResult.Data == null || !filesResult.Data.Any()) 
            {
                result.Data.StatusCode = StatusCodes.Status404NotFound;
                result.StatusCode = StatusCodes.Status404NotFound;
                return result;
            }

            result.Data.Files = filesResult.Data
                .Select(x => new FileDto() { FileName = x.FileName, Id = x.Id })
                .ToList();

            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;
            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetTaskFilesCommand request, CancellationToken ct)
        {
            if (request.TaskId == null || request.TaskId == Guid.Empty) 
            {
                throw new ProcessingException("The task Id must be filled in.");
            }

            return Task.CompletedTask;
        }
    }
}
