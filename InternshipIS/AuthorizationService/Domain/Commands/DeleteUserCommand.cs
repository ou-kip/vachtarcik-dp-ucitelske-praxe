using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// The command for deleting the user
    /// </summary>
    public class DeleteUserCommand : BaseRequest<DeleteUserResponse>
    {
        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }
    }
}
