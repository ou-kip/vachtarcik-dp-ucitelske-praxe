using Core.Domain.CommandHandlers;
using Core.Infrastructure.Results;
using InternshipService.Database.Repositories;
using InternshipService.Domain.Commands;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for getting name of companies
    /// </summary>
    public class GetCompaniesCommandHandler : BaseCommandHandler<GetCompaniesCommand, GetCompaniesResponse>
    {
        private readonly IInternshipCompanyRelativeRepository _companyRelativeRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="companyRelativeRepository"></param>
        /// <param name="logger"></param>
        public GetCompaniesCommandHandler(
            IInternshipCompanyRelativeRepository companyRelativeRepository,
            ILogger<GetCompaniesCommandHandler> logger) : base(logger)
        {
            _companyRelativeRepository = companyRelativeRepository;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<GetCompaniesResponse>> HandleAsync(GetCompaniesCommand request, CancellationToken ct)
        {
            var result = new ApiResult<GetCompaniesResponse>();
            var companyNames = await _companyRelativeRepository.GetCompaniesCollectionAsync(ct);

            result.Data.CompanyNames = companyNames.ToList();
            result.Data.StatusCode = StatusCodes.Status200OK;
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(GetCompaniesCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
