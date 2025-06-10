using Core.Domain.Responses;

namespace AuthorizationService.Domain.Responses
{
    public class GetUserResponse : BaseResponse
    {
        public GetUserResponse() { }

        public GetUserResponse(string? role, string? fullName) 
        {
            Role = role;
            FullName = fullName;
        }
        public string? Role {  get; }
        public string? FullName { get; set; }
    }
}
