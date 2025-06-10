using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetCompanyRelativesResponse : BaseResponse
    {
        public GetCompanyRelativesResponse() { }

        public List<CompanyRelativeDto> CompanyRelatives { get; set; } = new List<CompanyRelativeDto>();
    }
}
