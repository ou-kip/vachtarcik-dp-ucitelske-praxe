using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;
using System.Text.Json.Serialization;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// The command for user registration
    /// </summary>
    public class RegisterUserCommand : BaseRequest<RegisterUserResponse>
    {
        /// <summary>
        /// .ctor for RegisterUserCommand
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public RegisterUserCommand(string? code, string name, string lastName, string email, string password, string? userName = null, string? companyName = "") 
        {
            UserName = userName;
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            Code = code;
            CompanyName = companyName;
        }

        /// <summary>
        /// The username or nickname
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// The user code
        /// </summary>
        public string? Code { get; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The name of the company
        /// </summary>
        public string? CompanyName { get; }

        /// <summary>
        /// The code of the role
        /// </summary>
        [JsonIgnore]
        public string RoleCode { get; private set; }

        /// <summary>
        /// Sets up the RoleCode
        /// </summary>
        /// <param name="roleCode"></param>
        public void SetRole(string roleCode) 
        {
            if (!string.IsNullOrEmpty(roleCode))
            {
                RoleCode = roleCode;
                return;
            }

            throw new ArgumentException($"Role for: {UserName} is already set!");
        }
    }
}
