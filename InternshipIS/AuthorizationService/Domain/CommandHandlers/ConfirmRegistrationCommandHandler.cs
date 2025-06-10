using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Infrastructure.Results;

namespace AuthorizationService.Domain.CommandHandlers
{
    /// <summary>
    /// Handles the confirmation of user registration by validating the provided token.
    /// </summary>
    public class ConfirmRegistrationCommandHandler : BaseCommandHandler<ConfirmRegistrationCommand, ConfirmRegistrationResponse>
    {
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="logger"></param>
        public ConfirmRegistrationCommandHandler(IAuthorizationService authorizationService, ILogger<ConfirmRegistrationCommandHandler> logger) : base(logger)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<ConfirmRegistrationResponse>> HandleAsync(ConfirmRegistrationCommand request, CancellationToken ct)
        {
            _ = await _authorizationService.ConfirmUserEmail(request.Token, ct);

            return new ApiResult<ConfirmRegistrationResponse>
            {
                Data = new ConfirmRegistrationResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                },
                StatusCode = (int)StatusCodes.Status200OK,
            };
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(ConfirmRegistrationCommand request, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(request.Token))
            {
                throw new ArgumentException("Confirmation token cannot be null or empty.", nameof(request.Token));
            }

            return Task.CompletedTask;
        }
    }
}
