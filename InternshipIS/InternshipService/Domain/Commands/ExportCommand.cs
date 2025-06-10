using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class ExportCommand : BaseRequest<ExportResponse>
    {
        public Guid InternshipId { get; set; }
    }
}
