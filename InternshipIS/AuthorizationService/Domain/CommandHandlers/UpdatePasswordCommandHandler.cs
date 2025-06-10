using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Infrastructure.Results;

namespace AuthorizationService.Domain.CommandHandlers
{
    public class UpdatePasswordCommandHandler : BaseCommandHandler<UpdatePasswordCommand, UpdatePasswordResponse>
    {
        private readonly IAuthorizationService _authorizationService;

        public UpdatePasswordCommandHandler(
            IAuthorizationService authorizationService,
            ILogger<UpdatePasswordCommandHandler> logger
            ) : base(logger)
        {
            _authorizationService = authorizationService;
        }

        protected override async Task<IApiResult<UpdatePasswordResponse>> HandleAsync(UpdatePasswordCommand request, CancellationToken ct)
        {
            await _authorizationService.UpdatePasswordAsync(request.Token, request.Password, ct);
            return new ApiResult<UpdatePasswordResponse>()
            {
                Data = new() { StatusCode = StatusCodes.Status200OK }
            };
        }

        protected override Task ValidateDataAsync(UpdatePasswordCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
