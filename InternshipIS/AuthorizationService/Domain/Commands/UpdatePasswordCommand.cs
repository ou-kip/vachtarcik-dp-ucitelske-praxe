using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// The command for updating the password
    /// </summary>
    public class UpdatePasswordCommand : BaseRequest<UpdatePasswordResponse>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="password"></param>
        public UpdatePasswordCommand(string token, string password) 
        {
            Token = token;
            Password = password;
        }

        /// <summary>
        /// The generated token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// The user password
        /// </summary>
        public string Password { get; }
    }
}
