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
    /// The command handler for uploading the file
    /// </summary>
    public class UploadTaskFileCommandHandler : BaseCommandHandler<UploadTaskFileCommand, UploadFileResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;
        private readonly IInternshipTaskFileRepository _internshipTaskFileRepository;
        private readonly IFileService _fileService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipTaskRepository"></param>
        /// <param name="internshipTaskFileRepository"></param>
        /// <param name="fileService"></param>
        /// <param name="logger"></param>
        public UploadTaskFileCommandHandler(
            IInternshipTaskRepository internshipTaskRepository,
            IInternshipTaskFileRepository internshipTaskFileRepository,
            IFileService fileService,
            ILogger<UploadTaskFileCommandHandler> logger) : base(logger)
        {
            _internshipTaskFileRepository = internshipTaskFileRepository;
            _internshipTaskRepository = internshipTaskRepository;
            _fileService = fileService;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<UploadFileResponse>> HandleAsync(UploadTaskFileCommand request, CancellationToken ct)
        {
            var result = new ApiResult<UploadFileResponse>();

            var taskResult = _internshipTaskRepository.Get(request.TaskId);
            if (taskResult.HasErrors || taskResult.Data == null)
            {
                throw new NotFoundException(typeof(InternshipTask).Name, request.TaskId.ToString());
            }

            var task = taskResult.Data;

            var fileResult = await _fileService.UploadAsync(task.Id.ToString(), request.File, ct);
            if (fileResult.HasErrors) 
            {
                throw new ProcessingException(fileResult.GetErrorMessage());
            }

            var taskFile = new InternshipTaskFile
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                CreationAuthor = request.PerformerEmail,
                CreationDate = DateTime.Now,
                UpdateAuthor = request.PerformerEmail,
                UpdatedDate = DateTime.Now,
                Task = task,
                Extension = "",
                FileNameWithoutExt = Path.GetFileNameWithoutExtension(request.File.FileName)
            };

            task.Files.Add(taskFile);

            await _internshipTaskFileRepository.InsertAsync(taskFile, ct);
            _internshipTaskRepository.Update(task);

            await _internshipTaskFileRepository.SaveAsync(ct);
            await _internshipTaskRepository.SaveAsync(ct);

            result.StatusCode = StatusCodes.Status201Created;
            result.Data.StatusCode = StatusCodes.Status201Created;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(UploadTaskFileCommand request, CancellationToken ct)
        {
            if (request.TaskId == null || request.TaskId == Guid.Empty) 
            {
                throw new ProcessingException("TaskId must be filled.");
            }
            if(request.File == null)
            {
                throw new ProcessingException("No file selected for upload.");
            }

            return Task.CompletedTask;
        }
    }
}
