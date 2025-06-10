using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for deletion of internship
    /// </summary>
    public class DeleteCommandHandler : BaseInternshipCommandHandler<DeleteCommand, DeleteResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="internshipTaskRepository"></param>
        /// <param name="logger"></param>
        public DeleteCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            IInternshipCategoryRepository internshipCategoryRepository, 
            IInternshipLinkRepository internshipLinkRepository,
            IInternshipTaskRepository internshipTaskRepository,
            ILogger<DeleteCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<DeleteResponse>> HandleAsync(DeleteCommand request, CancellationToken ct)
        {
            var result = new ApiResult<DeleteResponse>();

            var internshipResult = await InternshipRepository.GetWithTasksByIdAsync(request.Id, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                throw new NotFoundException(typeof(Internship).Name, request.Id.ToString());
            }

            var deleteResult = InternshipRepository.Delete(internshipResult.Data);
            await InternshipRepository.SaveAsync(ct);

            foreach (var task in internshipResult.Data.Tasks)
            {
                _internshipTaskRepository.Delete(task);
            }

            if (!deleteResult.HasErrors) 
            {
                result.Data.Deleted = true;
            }

            await _internshipTaskRepository.SaveAsync(ct);

            result.Data.StatusCode = StatusCodes.Status204NoContent;
            result.StatusCode = StatusCodes.Status204NoContent;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(DeleteCommand request, CancellationToken ct)
        {
            if (request.Id == null || request.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Internship Id must be filled.");
            }

            return Task.CompletedTask;
        }
    }
}
