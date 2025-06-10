using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetTaskCommand : BaseRequest<GetTaskResponse>
    {
        public Guid Id { get; set; }
    }
}
