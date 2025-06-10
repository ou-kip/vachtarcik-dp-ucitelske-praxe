using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetTasksCommand : BaseRequest<GetTasksResponse>
    {
        public Guid InternshipId { get; set; }
    }
}
