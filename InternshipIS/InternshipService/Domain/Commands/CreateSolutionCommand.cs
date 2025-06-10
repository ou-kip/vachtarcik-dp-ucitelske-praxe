using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class CreateSolutionCommand : BaseRequest<CreateSolutionResponse>
    {
        public Guid TaskId { get; set; }
        public string Solution { get; set; }
    }
}
