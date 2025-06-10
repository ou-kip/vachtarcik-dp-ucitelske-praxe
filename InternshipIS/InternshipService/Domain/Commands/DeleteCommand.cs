using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class DeleteCommand : BaseRequest<DeleteResponse>
    {
        public Guid Id { get; set; }
    }
}
