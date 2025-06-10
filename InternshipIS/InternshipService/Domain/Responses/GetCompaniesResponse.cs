using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetCompaniesResponse : BaseResponse
    {
        public GetCompaniesResponse() { }

        public List<string> CompanyNames { get; set; }
    }
}
