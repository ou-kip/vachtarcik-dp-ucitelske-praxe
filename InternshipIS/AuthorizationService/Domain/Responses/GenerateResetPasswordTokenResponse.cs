using Core.Domain.Responses;

namespace AuthorizationService.Domain.Responses
{
    public class GenerateResetPasswordTokenResponse : BaseResponse
    {
        public string Identifier { get; set; }
    }
}
