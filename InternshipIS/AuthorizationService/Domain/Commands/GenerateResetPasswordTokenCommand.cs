using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// The command for resetting the password
    /// </summary>
    public class GenerateResetPasswordTokenCommand : BaseRequest<GenerateResetPasswordTokenResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="email"></param>
        public GenerateResetPasswordTokenCommand(string email) 
        {
            Email = email;
        }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; }
    }
}
