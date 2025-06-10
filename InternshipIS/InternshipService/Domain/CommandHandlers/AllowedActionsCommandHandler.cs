using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for notificating whethet user has allowed actions on internship or its child entities
    /// </summary>
    public class AllowedActionsCommandHandler : BaseInternshipCommandHandler<AllowedActionsCommand, AllowedActionsResponse>
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
        public AllowedActionsCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            IInternshipCategoryRepository internshipCategoryRepository, 
            IInternshipLinkRepository internshipLinkRepository, 
            ILogger<AllowedActionsCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<AllowedActionsResponse>> HandleAsync(AllowedActionsCommand request, CancellationToken ct)
        {
            var result = new ApiResult<AllowedActionsResponse>();

            var internshipResult = await InternshipRepository.GetAsync(request.InternshipId, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null) 
            {
                throw new ProcessingException($"Unable to get Internship with ID: {request.InternshipId}");
            }

            var internship = internshipResult.Data;

            var allowedActions = new AllowedActionsDto();

            if (internship.Students.Any() && internship.Students.Select(x => x.Email.ToLowerInvariant()).Contains(request.PerformerEmail.ToLowerInvariant()))
            {
                allowedActions.Allowed = true;
                allowedActions.IsStudent = true;
            }
            else if (internship.Teachers.Any() && internship.Teachers.Select(x => x.Email.ToLowerInvariant()).Contains(request.PerformerEmail.ToLowerInvariant()))
            {
                allowedActions.Allowed = true;
                allowedActions.IsTeacher = true;
            }
            else if (internship.CompanyRelatives.Any() && internship.CompanyRelatives.Select(x => x.Email.ToLowerInvariant()).Contains(request.PerformerEmail.ToLowerInvariant()))
            {
                allowedActions.Allowed = true;
                allowedActions.IsCompanyRelative = true;
            }
            else
            {
                allowedActions.Allowed = false;
            }

            result.Data.AllowedActions = allowedActions;
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(AllowedActionsCommand request, CancellationToken ct)
        {
            if (request.InternshipId == null || request.InternshipId == Guid.Empty) 
            {
                throw new ArgumentNullException(nameof(request.InternshipId), "The value of InternshipID must be provided!");
            }
            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                throw new ArgumentNullException("Unable to get requestor email!");
            }

            return Task.CompletedTask;
        }
    }
}
