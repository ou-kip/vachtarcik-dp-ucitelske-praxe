using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    /// <summary>
    /// The response which returns collection of internships
    /// </summary>
    public class GetCollectionResponse : BaseResponse
    {
        /// <summary>
        /// The intenrship collection
        /// </summary>
        public List<InternshipDto> Items { get; set; } = new List<InternshipDto>();
    }
}
