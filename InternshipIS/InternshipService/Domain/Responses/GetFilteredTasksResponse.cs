using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetFilteredTasksResponse : BaseResponse
    {
        public GetFilteredTasksResponse() { }

        public List<RichInternshipTaskDto> Tasks { get; set; }
    }
}
