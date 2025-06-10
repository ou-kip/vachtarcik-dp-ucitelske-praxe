using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class CreateSolutionResponse : BaseResponse
    {
        public Guid SolutionId { get; set; }
    }
}
