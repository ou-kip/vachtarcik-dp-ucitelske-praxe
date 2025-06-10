using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the collection with students
    /// </summary>
    public class GetStudentsCommandHandler : BaseInternshipPersonCommandHandler<GetStudentsCommand, GetStudentsResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="logger"></param>
        public GetStudentsCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            ILogger<GetStudentsCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository,internshipStudentRepository, internshipCompanyRelativeRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetStudentsResponse>> HandleAsync(GetStudentsCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetStudentsResponse>();

            var studentsResult = await InternshipStudentRepository.GetAllAsync(ct: ct);
            if (studentsResult.HasErrors) 
            {
                throw new ProcessingException(studentsResult.GetErrorMessage());
            }

            result.Data.Students = studentsResult.Data
                .Select(x => new StudentDto(x.Id, x.Name, x.LastName))
                .ToList();

            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetStudentsCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
