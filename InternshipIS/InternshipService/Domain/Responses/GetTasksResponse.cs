using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetTasksResponse : BaseResponse
    {
        public List<InternshipTaskDto> Tasks { get; set; }
    }
}
