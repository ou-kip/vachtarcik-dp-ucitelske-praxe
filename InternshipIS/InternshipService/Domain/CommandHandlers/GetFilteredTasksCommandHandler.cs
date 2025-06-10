using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the filtered collection of tasks
    /// </summary>
    public class GetFilteredTasksCommandHandler : BaseInternshipCommandHandler<GetFilteredTasksCommand, GetFilteredTasksResponse>
    {
        private readonly IInternshipTaskRepository _internshipTaskRepository;

        public GetFilteredTasksCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            IInternshipTaskRepository internshipTaskRepository,
            ILogger<GetTasksCommandHandler> logger) : base(
                internshipRepository,
                internshipTeacherRepository,
                internshipStudentRepository,
                internshipCompanyRelativeRepository,
                internshipCategoryRepository,
                internshipLinkRepository,
                logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetFilteredTasksResponse>> HandleAsync(GetFilteredTasksCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetFilteredTasksResponse>();

            try
            {
                var query = _internshipTaskRepository.GetAllWithInternshipsQuery();

                if (request.CreatedByMe && !string.IsNullOrEmpty(request.PerformerEmail))
                {
                    query = query.Where(x =>
                    x.Internship.Teachers.Any(x => x.Email.Equals(request.PerformerEmail)) ||
                    x.Internship.CompanyRelatives.Any(x => x.Email.Equals(request.PerformerEmail)));
                }
                if (!string.IsNullOrEmpty(request.FilterProperty))
                {
                    query = ApplyFilter(query, request.FilterProperty, request.FilterValue);
                }
                if (!string.IsNullOrEmpty(request.OrderProperty))
                {
                    query = ApplyOrdering(query, request.OrderProperty, request.OrderDirection);
                }

                result.Data.Tasks = query.ToList().Select(x => new RichInternshipTaskDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    EndsOn = x.EndsOn,
                    IsCompleted = x.IsCompleted,
                    Name = x.Name,
                    State = x.State,
                    InternshipId = x.InternshipId,
                    InternshipName = x.Internship.Name
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
        protected override Task ValidateDataAsync(GetFilteredTasksCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
