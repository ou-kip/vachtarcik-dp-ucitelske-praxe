using Core.Database.Entities;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Enums;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for creating a new internship
    /// </summary>
    public class CreateCommandHandler : BaseInternshipCommandHandler<CreateCommand, CreateResponse>
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
        public CreateCommandHandler(
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
        protected override async Task<IApiResult<CreateResponse>> HandleAsync(CreateCommand request, CancellationToken ct = default)
        {
            var response = new ApiResult<CreateResponse>();

            var internship = new Internship()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CompanyName = request.CompanyName,
                CreationAuthor = request.PerformerEmail,
                CreationDate = DateTime.Now,
                UpdateAuthor = request.PerformerEmail,
                UpdatedDate = DateTime.Now,
                State = request.State,
            };

            await AddStudentsAsync(internship, request.StudentId, ct);
            await AddTeachersAsync(internship, request.TeacherIds, ct);
            await AddCompanyRelativesAsync(internship, request.CompanyRelativeIds, ct);
            await AddCategoryAsync(internship, request.CategoryId, ct);

            if(request.StudentId == null && request.StudentId != Guid.Empty)
            {
                internship.SelectedOn = DateTime.Now;
                internship.State = (int)InternshipStateEnum.Chosen;
            }

            await InternshipRepository.InsertAsync(internship, ct);
            await InternshipRepository.SaveAsync(ct);

            if (request.Links != null)
            {
                foreach (var link in request.Links)
                {
                    if (string.IsNullOrEmpty(link.Name)) { continue; }
                    if(string.IsNullOrEmpty(link.Url)) { continue; }

                    var linkEntity = new InternshipLink
                    {
                        Id = Guid.NewGuid(),
                        Name = link.Name,
                        Url = link.Url,
                        CreationAuthor = request.PerformerEmail,
                        CreationDate = DateTime.Now,
                        UpdateAuthor = request.PerformerEmail,
                        UpdatedDate = DateTime.Now,
                        InternshipId = internship.Id
                    };

                    await InternshipLinkRepository.InsertAsync(linkEntity, ct);
                }
            }

            await InternshipLinkRepository.SaveAsync(ct);

            response.StatusCode = StatusCodes.Status201Created;
            response.Data.StatusCode = StatusCodes.Status201Created;
            response.Data.Id = internship.Id;

            return response;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(CreateCommand request, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                Logger.LogError("INTERNSHIP: Possibly unauthorized - no email set from token.");
                throw new ProcessingException("INTERNSHIP: Possibly unauthorized - no email set from token.");
            }

            if (string.IsNullOrEmpty(request.Name)) 
            {
                Logger.LogError("INTERNSHIP: Name of internship can not be empty.");
                throw new ProcessingException("INTERNSHIP: Name of the internship can not be empty.");
            }

            return Task.CompletedTask;
        }
    }
}
