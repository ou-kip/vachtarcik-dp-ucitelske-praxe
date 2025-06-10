using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetTaskFilesCommand : BaseRequest<GetTaskFilesResponse>
    {
        public GetTaskFilesCommand() { }
        public Guid TaskId { get; set; }
    }
}
