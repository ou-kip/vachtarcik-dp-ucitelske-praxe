using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for gettign the internship and its details
    /// </summary>
    public class GetInternshipCommandHandler : BaseInternshipCommandHandler<GetInternshipCommand, GetInternshipResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="logger"></param>
        public GetInternshipCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipLinkRepository internshipLinkRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            ILogger<CreateCommandHandler> logger)
            : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetInternshipResponse>> HandleAsync(GetInternshipCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetInternshipResponse>();

            var internshipResult = await InternshipRepository.GetAsync((Guid)request.InternshipId, ct);
            if (internshipResult.HasErrors || internshipResult.Data == null) 
            {
                throw new NotFoundException(typeof(Internship).Name, request.InternshipId.ToString());
            }

            var internship = internshipResult.Data;

            var internshipDto = new InternshipDto()
            {
                Id = internship.Id,
                Name = internship.Name,
                CompanyName = internship.CompanyName,
                CompanyRelatives = internship.CompanyRelatives?.Select(x => new CompanyRelativeDto(x.Id, x.Name, x.LastName)).ToList(),
                Teachers = internship.Teachers?.Select(x => new TeacherDto(x.Id, x.Name, x.LastName)).ToList(),
                Student = internship.Students?.Select(x => new StudentDto(x.Id, x.Name, x.LastName)).FirstOrDefault(),
                Links = internship.Links?.Select(x => new LinkDto(x.Id, x.Name, x.Url)).ToList(),
                CreatedByName = await GetAuthorNameAsync(internship.CreationAuthor, ct),
                Description = internship.Description,
                SelectedOn = internship.SelectedOn,
                State = internship.State,
                Category = internship.InternshipCategory != null ? new CategoryDto(internship.InternshipCategory.Id, internship.InternshipCategory.Code, internship.InternshipCategory.Name) : null
            };

            result.Data.InternshipDto = internshipDto;
            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetInternshipCommand request, CancellationToken ct)
        {
            if (request.InternshipId == Guid.Empty || request.InternshipId == null) 
            {  
                throw new ArgumentNullException(nameof(request.InternshipId), "Internship Id must be filled in order to get Internship details");
            }

            return Task.CompletedTask;
        }
    }
}
