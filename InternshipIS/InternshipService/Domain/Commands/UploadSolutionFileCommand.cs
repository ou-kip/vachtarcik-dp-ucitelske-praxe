using Core.Domain.Requests;
using InternshipService.Domain.Responses;
using System.ComponentModel.DataAnnotations;

namespace InternshipService.Domain.Commands
{
    public class UploadSolutionFileCommand : BaseRequest<UploadFileResponse>
    {
        public Guid SolutionId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
