using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;

namespace AuthorizationService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for resetting the password
    /// </summary>
    public class GenerateResetPasswordTokenCommandHandler : BaseCommandHandler<GenerateResetPasswordTokenCommand, GenerateResetPasswordTokenResponse>
    {
        private readonly IAuthorizationService _authorizationService;

        public GenerateResetPasswordTokenCommandHandler(IAuthorizationService authorizationService, ILogger<GenerateResetPasswordTokenCommandHandler> logger) : base(logger)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        /// <inheritdoc/>
        protected override async Task<IApiResult<GenerateResetPasswordTokenResponse>> HandleAsync(GenerateResetPasswordTokenCommand request, CancellationToken ct)
        {
            var apiResult = new ApiResult<GenerateResetPasswordTokenResponse>();

            var token = await _authorizationService.GenerateResetPasswordTokenAsync(request.Email);
            if (string.IsNullOrEmpty(token)) { throw new ProcessingException("Error during generation of reset password token"); }

            apiResult.Data.Identifier = token;
            return apiResult;
        }

        /// <inheritdoc/>
        protected override Task ValidateDataAsync(GenerateResetPasswordTokenCommand request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.Email)) { throw new ArgumentNullException(request.Email); }
            return Task.CompletedTask;
        }
    }
}
