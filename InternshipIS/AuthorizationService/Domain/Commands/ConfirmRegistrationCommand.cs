using AuthorizationService.Domain.Responses;
using Core.Domain.Requests;

namespace AuthorizationService.Domain.Commands
{
    /// <summary>
    /// Command to confirm user registration.
    /// </summary>
    public class ConfirmRegistrationCommand : BaseRequest<ConfirmRegistrationResponse>
    {
        /// <summary>
        /// The token used to confirm the registration.
        /// </summary>
        public string Token { get; set; }
    }
}
