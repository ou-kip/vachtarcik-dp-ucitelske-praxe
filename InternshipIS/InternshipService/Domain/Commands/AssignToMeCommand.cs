using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class AssignToMeCommand : BaseRequest<AssignToMeResponse>
    {
        public Guid InternshipId { get; set; }
    }
}
