using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetTaskFilesResponse : BaseResponse
    {
        public GetTaskFilesResponse() { }

        public List<FileDto> Files { get; set; }
    }
}
