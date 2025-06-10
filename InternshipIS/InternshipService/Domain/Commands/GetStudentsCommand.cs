using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetStudentsCommand : BaseRequest<GetStudentsResponse>
    {
        public GetStudentsCommand() { }

        public Guid? InternshipId { get; set; }
    }
}
