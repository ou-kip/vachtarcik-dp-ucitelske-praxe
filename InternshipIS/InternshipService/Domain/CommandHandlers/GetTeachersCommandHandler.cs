using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the collection of teachers
    /// </summary>
    public class GetTeachersCommandHandler : BaseInternshipPersonCommandHandler<GetTeachersCommand, GetTeachersResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="logger"></param>
        public GetTeachersCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            ILogger<GetTeachersCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetTeachersResponse>> HandleAsync(GetTeachersCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetTeachersResponse>();
            
            var teachersResult = await InternshipTeacherRepository.GetAllAsync(ct: ct);
            if (teachersResult.HasErrors)
            {
                throw new ProcessingException(teachersResult.GetErrorMessage());
            }

            var teacherDtos = teachersResult.Data
                .Select(x => { return new TeacherDto(x.Id, x.Name, x.LastName); })
                .ToList();

            result.Data.Teachers = teacherDtos;
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetTeachersCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
