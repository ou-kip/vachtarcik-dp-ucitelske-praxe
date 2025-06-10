using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    public class GetTeachersResponse : BaseResponse
    {
        /// <summary>
        /// The intenrship collection
        /// </summary>
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
    }
}
