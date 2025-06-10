using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using Core.Services;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for uploading files to the task solution
    /// </summary>
    public class UploadSolutionFileCommandHandler : BaseCommandHandler<UploadSolutionFileCommand, UploadFileResponse>
    {
        private readonly IInternshipTaskSolutionRepository _repository;
        private readonly IInternshipTaskSolutionFileRepository _fileRepository;
        private readonly IFileService _fileService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="fileRepository"></param>
        /// <param name="fileService"></param>
        /// <param name="logger"></param>
        public UploadSolutionFileCommandHandler(
            IInternshipTaskSolutionRepository repository,
            IInternshipTaskSolutionFileRepository fileRepository,
            IFileService fileService,
            ILogger<UploadSolutionFileCommandHandler> logger) : base(logger)
        {
            _repository = repository;
            _fileRepository = fileRepository;
            _fileService = fileService;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<UploadFileResponse>> HandleAsync(UploadSolutionFileCommand request, CancellationToken ct)
        {
            var result = new ApiResult<UploadFileResponse>();

            var solutionResult = await _repository.GetAsync(request.SolutionId);
            if (solutionResult.HasErrors || solutionResult.Data == null)
            {
                throw new NotFoundException(typeof(InternshipTaskSolution).Name, request.SolutionId.ToString());
            }

            var solution = solutionResult.Data;

            var fileResult = await _fileService.UploadAsync(solution.Id.ToString(), request.File, ct);
            if (fileResult.HasErrors)
            {
                throw new ProcessingException(fileResult.GetErrorMessage());
            }

            var solutionFile = new InternshipTaskSolutionFile
            {
                Id = Guid.NewGuid(),
                SolutionId = request.SolutionId,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                CreationAuthor = request.PerformerEmail,
                CreationDate = DateTime.Now,
                UpdateAuthor = request.PerformerEmail,
                UpdatedDate = DateTime.Now,
                Solution = solution,
                Extension = "",
                FileNameWithoutExt = Path.GetFileNameWithoutExtension(request.File.FileName)
            };

            solution.Files.Add(solutionFile);

            await _fileRepository.InsertAsync(solutionFile, ct);
            _repository.Update(solution);

            await _fileRepository.SaveAsync(ct);
            await _repository.SaveAsync(ct);

            result.StatusCode = StatusCodes.Status201Created;
            result.Data.StatusCode = StatusCodes.Status201Created;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(UploadSolutionFileCommand request, CancellationToken ct)
        {
            if (request.SolutionId == null || request.SolutionId == Guid.Empty)
            {
                throw new ProcessingException("SolutionId must be filled.");
            }
            if (request.File == null)
            {
                throw new ProcessingException("No file selected for upload.");
            }

            return Task.CompletedTask;
        }
    }
}
