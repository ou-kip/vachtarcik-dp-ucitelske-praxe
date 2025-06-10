using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;
using System.Text.Json.Serialization;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// The command for user login
    /// </summary>
    public class LoginCommand : BaseRequest<LoginResponse>
    {
        /// <summary>
        /// .ctor for LoginCommand
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginCommand(string email, string password) 
        { 
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        /// <summary>
        /// The user email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// The user password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The token
        /// </summary>
        [JsonIgnore]
        public string? Token { get; private set; }

        /// <summary>
        /// Sets the Token
        /// </summary>
        public void SetToken(string? token)
        {
            Token = token ?? null;
        }
    }
}
