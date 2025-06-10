using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class ExportResponse : BaseResponse
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
