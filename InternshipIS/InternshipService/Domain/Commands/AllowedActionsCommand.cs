using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class AllowedActionsCommand : BaseRequest<AllowedActionsResponse>
    {
        public Guid InternshipId { get; set; }
    }
}
