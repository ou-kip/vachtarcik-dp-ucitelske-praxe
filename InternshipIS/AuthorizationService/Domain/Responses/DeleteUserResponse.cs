using Core.Domain.Responses;

namespace AuthorizationService.Domain.Responses
{
    /// <summary>
    /// The delete user response
    /// </summary>
    public class DeleteUserResponse : BaseResponse
    {
        public bool Deleted {  get; set; }
    }
}
