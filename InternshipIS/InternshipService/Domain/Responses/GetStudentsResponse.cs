using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetStudentsResponse : BaseResponse
    {
        /// <summary>
        /// The intenrship students collection
        /// </summary>
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}
