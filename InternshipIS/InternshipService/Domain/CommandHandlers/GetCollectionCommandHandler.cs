using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the collection of Internships
    /// </summary>
    public class GetCollectionCommandHandler : BaseInternshipCommandHandler<GetCollectionCommand, GetCollectionResponse>
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
        public GetCollectionCommandHandler(
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
        protected override async Task<IApiResult<GetCollectionResponse>> HandleAsync(GetCollectionCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetCollectionResponse>();

            var queryResult = await InternshipRepository.GetAllAsQueryableAsync(ct);
            if (queryResult.HasErrors) 
            {
                throw new ProcessingException(queryResult.GetErrorMessage());
            }

            var query = queryResult.Query;

            if (!string.IsNullOrEmpty(request.FilterProperty))
            {
                query = ApplyFilter(query, request.FilterProperty, request.FilterValue);
            }

            if (request.CreatedByMe && !string.IsNullOrEmpty(request.PerformerEmail))
            {
                query = ApplyCreatedByMeFilter(query, request.PerformerEmail);
            }
            if (!string.IsNullOrEmpty(request.OrderProperty))
            {
                query = ApplyOrdering(query, request.OrderProperty, request.OrderDirection);
            }

            //materialization
            var data = query.ToList();

            var dtoCollection = new List<InternshipDto>();

            foreach (var internship in data) 
            {
                string studentName = string.Empty;
                string teacherName = string.Empty;
                string companyRelativeName = string.Empty;


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
                    Category = internship.InternshipCategory != null ? new CategoryDto(internship.InternshipCategory.Id, internship.InternshipCategory.Code, internship.InternshipCategory.Name) : null,
                    EndsOn = internship.FinishedOn
                };

                if (internship.CreationAuthor.Equals(request.PerformerEmail))
                {
                    internshipDto.IsCreatedByRequester = true;
                }

                dtoCollection.Add(internshipDto);
            }

            result.Data.Items = dtoCollection;
            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetCollectionCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
