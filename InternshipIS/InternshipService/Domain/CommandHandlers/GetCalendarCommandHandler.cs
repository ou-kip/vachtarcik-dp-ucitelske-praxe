using Core.Domain.CommandHandlers;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Enums;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// Command handler for getting the calendar events
    /// </summary>
    public class GetCalendarCommandHandler : BaseInternshipCommandHandler<GetCalendarCommand, GetCalendarResponse>
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
        public GetCalendarCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            IInternshipCategoryRepository internshipCategoryRepository, 
            IInternshipLinkRepository internshipLinkRepository,
            IInternshipTaskRepository internshipTaskRepository,
            ILogger<GetCalendarCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, internshipCategoryRepository, internshipLinkRepository, logger)
        {
            _internshipTaskRepository = internshipTaskRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetCalendarResponse>> HandleAsync(GetCalendarCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetCalendarResponse>();

            try
            {
                var query = _internshipTaskRepository.GetAllWithInternshipsQuery();
                var events = new List<CalendarEventDto>();

                var tasks = query
                    .Where(x =>
                        x.Internship.Teachers.Any(x => x.Email.Equals(request.PerformerEmail)) ||
                        x.Internship.CompanyRelatives.Any(x => x.Email.Equals(request.PerformerEmail)) ||
                        x.Internship.Students.Any(x => x.Email.Equals(request.PerformerEmail)))
                    .OrderBy(x=>x.EndsOn)
                    .ToList();

                foreach (var task in tasks) 
                {
                    events.Add(new()
                    {
                        EventId = task.Id,
                        EventName = $"Termín odevzdání úkolu: {task.Name}",
                        EventType = (int)EventTypeEnum.Task,
                        EventTime = task.EndsOn.ToString()
                    });
                }

                result.Data.Events = events;
                result.Data.StatusCode = StatusCodes.Status200OK;
                result.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex) 
            {
                Logger.LogError(ex.Message);
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Data.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetCalendarCommand request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.PerformerEmail))
            {
                Logger.LogError("Performer email does not have a value for getting the calendar.");
                throw new ArgumentNullException(nameof(request));
            }

            return Task.CompletedTask;
        }
    }
}
