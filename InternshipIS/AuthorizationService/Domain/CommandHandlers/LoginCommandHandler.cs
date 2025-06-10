using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Dto;
using Core.Infrastructure.Results;

namespace AuthorizationService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for user login
    /// </summary>
    public class LoginCommandHandler : BaseCommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IClaimsProcessor _claimsProcessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITokenMemoryCache _tokenMemoryCache;

        /// <summary>
        /// .ctor for LoginCommandHandler
        /// </summary>
        /// <param name="claimsProcessor"></param>
        /// <param name="authorizationService"></param>
        /// <param name="logger"></param>
        public LoginCommandHandler(
            IClaimsProcessor claimsProcessor,
            IAuthorizationService authorizationService,
            ILogger<LoginCommandHandler> logger) : base(logger)
        {
            _authorizationService = authorizationService;
            _claimsProcessor = claimsProcessor;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<LoginResponse>> HandleAsync(LoginCommand request, CancellationToken ct)
        {
            var apiResult = new ApiResult<LoginResponse>();

            var dto = new LoginDto(request.Email, request.Password, request.Token);
            var result = await _authorizationService.AuthorizeUsingJwtAsync(dto, ct);

            if (string.IsNullOrEmpty(result)) 
            {
                apiResult.Data.Cookie = null;
                return apiResult;
            }

            apiResult.Data.Cookie = await _authorizationService.CreateCookieOptionsAsync(DateTime.Now.AddHours(2), ct: ct);
            apiResult.Data.Token = result;

            return apiResult;
        }

        ///<inheritdoc/>
        protected override Task ValidateDataAsync(LoginCommand request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
