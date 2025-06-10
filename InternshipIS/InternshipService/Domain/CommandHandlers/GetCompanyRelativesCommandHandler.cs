using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting the company relative persons
    /// </summary>
    public class GetCompanyRelativesCommandHandler : BaseCommandHandler<GetCompanyRelativesCommand, GetCompanyRelativesResponse>
    {
        private readonly IInternshipCompanyRelativeRepository _companyRelativeRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="companyRelativeRepository"></param>
        /// <param name="logger"></param>
        public GetCompanyRelativesCommandHandler(
            IInternshipCompanyRelativeRepository companyRelativeRepository,
            ILogger<GetCompanyRelativesCommandHandler> logger) : base(logger)
        {
            _companyRelativeRepository = companyRelativeRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetCompanyRelativesResponse>> HandleAsync(GetCompanyRelativesCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetCompanyRelativesResponse>();            
            
            var companyRelativesResult = await _companyRelativeRepository.GetAllAsync(ct: ct);
            if (companyRelativesResult.HasErrors) 
            {
                throw new ProcessingException(companyRelativesResult.GetErrorMessage());
            }


            var companyRelativesDtos = companyRelativesResult.Data
                .Select(x => { return new CompanyRelativeDto(x.Id, x.Name, x.LastName); })
                .ToList();

            result.Data.CompanyRelatives = companyRelativesDtos;
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetCompanyRelativesCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
