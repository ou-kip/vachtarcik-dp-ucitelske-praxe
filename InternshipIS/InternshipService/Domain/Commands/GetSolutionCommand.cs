using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetSolutionCommand : BaseRequest<GetSolutionResponse>
    {
        public Guid TaskId { get; set; }
    }
}
