using Core.Domain.Responses;

namespace InternshipService.Domain.Responses
{
    /// <summary>
    /// The response for internship create command
    /// </summary>
    public class CreateResponse : BaseResponse
    {
        /// <summary>
        /// The id of the created internship
        /// </summary>
        public Guid Id { get; set; }
    }
}
