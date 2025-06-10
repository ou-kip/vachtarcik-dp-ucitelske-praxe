using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetInternshipResponse : BaseResponse
    {
        public GetInternshipResponse() { }

        public InternshipDto InternshipDto { get; set; }
    }
}
