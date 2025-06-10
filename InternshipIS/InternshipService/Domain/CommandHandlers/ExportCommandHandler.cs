using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using InternshipService.Services;
using System.Text;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The class <c>ExportCommandHandler</c> handles the export command for internships.
    /// </summary>
    public class ExportCommandHandler : BaseInternshipCommandHandler<ExportCommand, ExportResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="logger"></param>
        public ExportCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            ILogger<ExportCommandHandler> logger)
            : base(
                  internshipRepository,
                  internshipTeacherRepository,
                  internshipStudentRepository,
                  internshipCompanyRelativeRepository,
                  internshipCategoryRepository,
                  internshipLinkRepository,
                  logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<ExportResponse>> HandleAsync(ExportCommand request, CancellationToken ct)
        {
            var internshipResult = await InternshipRepository.GetWithAllDataByIdAsync(request.InternshipId, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null)
            {
                Logger.LogError("Failed to retrieve internship data for ID: {InternshipId}", request.InternshipId);
                throw new ProcessingException("Unable to get internship data.");
            }

            var internship = internshipResult.Data;

            var htmlContent = ExportBuilder.Build(internship);
            var htmlBytes = Encoding.UTF8.GetBytes(htmlContent);
            var fileName = $"report-{internship.Name}.html";

            return new ApiResult<ExportResponse>()
            {
                Data = new() { FileName = fileName, FileContent = htmlBytes }
            };
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(ExportCommand request, CancellationToken ct)
        {
            if(request.InternshipId == Guid.Empty)
            {
                throw new ArgumentException("InternshipId cannot be empty.", nameof(request.InternshipId));
            }

            return Task.CompletedTask;
        }
    }
}
