using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Exceptions;
using Core.Infrastructure.Results;

namespace AuthorizationService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for deleting the user
    /// </summary>
    public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="logger"></param>
        public DeleteUserCommandHandler(IAuthorizationService authService, ILogger<DeleteUserCommandHandler> logger) : base(logger)
        {
            _authService = authService;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<DeleteUserResponse>> HandleAsync(DeleteUserCommand request, CancellationToken ct)
        {
            var result = new ApiResult<DeleteUserResponse>();
            result.Data.Deleted = await _authService.DeleteUserAsync(request.Email.ToString(), ct);

            result.StatusCode = StatusCodes.Status200OK;
            result.Data.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(DeleteUserCommand request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new ProcessingException("The email value is not present.");
            }

            return Task.CompletedTask;
        }
    }
}
