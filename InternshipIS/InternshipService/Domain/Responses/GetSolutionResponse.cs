using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetSolutionResponse : BaseResponse
    {
        public SolutionDto Solution { get; set; }
    }
}
