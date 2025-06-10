using Contracts;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// Command handler for getting all persons based on type
    /// </summary>
    public class GetAllPersonsCommandHandler : BaseInternshipPersonCommandHandler<GetAllPersonsCommand, GetAllPersonsResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="logger"></param>
        public GetAllPersonsCommandHandler(
            IInternshipRepository internshipRepository, 
            IInternshipTeacherRepository internshipTeacherRepository, 
            IInternshipStudentRepository internshipStudentRepository, 
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository, 
            ILogger<GetAllPersonsCommandHandler> logger) : base(internshipRepository, internshipTeacherRepository, internshipStudentRepository, internshipCompanyRelativeRepository, logger)
        {
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetAllPersonsResponse>> HandleAsync(GetAllPersonsCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetAllPersonsResponse>();
            result.Data.Accounts = new List<PersonDto>();

            //teachers
            if(request.PersonType == (int)InternshipPersonTypeEnum.Teacher)
            {
                if (!string.IsNullOrEmpty(request.Search))
                {
                    var queryResult = await InternshipTeacherRepository.GetAllAsQueryableAsync(ct);
                    if (queryResult.HasErrors) 
                    {
                        throw new ProcessingException(queryResult.GetErrorMessage());
                    }

                    var query = queryResult.Query.Where(x => x.Name.Contains(request.Search) || x.LastName.Contains(request.Search));
                    result.Data.Accounts = (await query.ToListAsync(ct)).Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.Teacher)).ToList();
                }
                else
                {
                    var teachersResult = await InternshipTeacherRepository.GetAllAsync(request.Offset, request.Take, ct);
                    if (teachersResult.HasErrors)
                    {
                        throw new ProcessingException(teachersResult.GetErrorMessage());
                    }

                    result.Data.Accounts = teachersResult.Data.Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.Teacher)).ToList();
                }
            }

            //students
            else if (request.PersonType == (int)InternshipPersonTypeEnum.Student)
            {
                if (!string.IsNullOrEmpty(request.Search))
                {
                    var queryResult = await InternshipStudentRepository.GetAllAsQueryableAsync(ct);
                    if (queryResult.HasErrors)
                    {
                        throw new ProcessingException(queryResult.GetErrorMessage());
                    }

                    var query = queryResult.Query.Where(x => x.Name.Contains(request.Search) || x.LastName.Contains(request.Search));
                    result.Data.Accounts = (await query.ToListAsync(ct)).Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.Student)).ToList();
                }
                else
                {
                    var studentsResult = await InternshipStudentRepository.GetAllAsync(request.Offset, request.Take, ct);
                    if (studentsResult.HasErrors)
                    {
                        throw new ProcessingException(studentsResult.GetErrorMessage());
                    }

                    result.Data.Accounts = studentsResult.Data.Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.Student)).ToList();
                }
            }

            //company relative persons
            else if (request.PersonType == (int)InternshipPersonTypeEnum.CompanyRelative)
            {
                if (!string.IsNullOrEmpty(request.Search))
                {
                    var queryResult = await InternshipCompanyRelativeRepository.GetAllAsQueryableAsync(ct);
                    if (queryResult.HasErrors)
                    {
                        throw new ProcessingException(queryResult.GetErrorMessage());
                    }

                    var query = queryResult.Query.Where(x => x.Name.Contains(request.Search) || x.LastName.Contains(request.Search));
                    result.Data.Accounts = (await query.ToListAsync(ct)).Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.CompanyRelative)).ToList();
                }
                else
                {
                    var relativesResult = await InternshipCompanyRelativeRepository.GetAllAsync(request.Offset, request.Take, ct);
                    if (relativesResult.HasErrors)
                    {
                        throw new ProcessingException(relativesResult.GetErrorMessage());
                    }

                    result.Data.Accounts = relativesResult.Data.Select(x => new PersonDto(x.Id, x.Name, x.LastName, x.Email, (int)InternshipPersonTypeEnum.CompanyRelative)).ToList();
                }
            }

            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;
            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetAllPersonsCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
