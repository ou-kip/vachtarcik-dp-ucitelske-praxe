using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetTaskResponse : BaseResponse
    {
        public GetTaskResponse() { }

        public InternshipTaskDto Task { get; set; }
    }
}
