using Core.Domain.Requests;
using InternshipService.Domain.Responses;
using System.ComponentModel.DataAnnotations;

namespace InternshipService.Domain.Commands
{
    public class UploadTaskFileCommand : BaseRequest<UploadFileResponse>
    {
        public Guid TaskId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
