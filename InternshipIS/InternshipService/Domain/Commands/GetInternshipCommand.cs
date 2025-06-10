using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetInternshipCommand : BaseRequest<GetInternshipResponse>
    {
        public GetInternshipCommand() { }

        public Guid? InternshipId { get; set; }
    }
}
