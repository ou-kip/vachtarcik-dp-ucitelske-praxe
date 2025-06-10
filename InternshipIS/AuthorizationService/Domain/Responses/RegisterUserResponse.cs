using Core.Domain.Responses;

namespace AuthorizationService.Domain.Responses
{
    /// <summary>
    /// The Register user response
    /// </summary>
    public class RegisterUserResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
