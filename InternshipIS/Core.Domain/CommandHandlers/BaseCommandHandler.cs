using Core.Domain.Requests;
using Core.Domain.Responses;
using Core.Infrastructure.Results;
using Microsoft.Extensions.Logging;

namespace Core.Domain.CommandHandlers
{
    /// <summary>
    /// The base class for command handlers
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseCommandHandler<TCommand, TResponse> : IBaseCommandHandler<TCommand, TResponse>
        where TCommand : IBaseRequest<TResponse>
        where TResponse : IBaseResponse
    {
        /// <summary>
        /// .base ctor 
        /// </summary>
        /// <param name="logger"></param>
        public BaseCommandHandler(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// The logger
        /// </summary>
        protected ILogger Logger { get; } 

        /// <summary>
        /// Handles the command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IApiResult<TResponse>> Handle(TCommand request, CancellationToken ct)
        {
            await ValidateDataAsync(request, ct);
            return await HandleAsync(request, ct);
        }

        /// <summary>
        /// Handles the command internally
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected abstract Task<IApiResult<TResponse>> HandleAsync(TCommand request, CancellationToken ct);

        /// <summary>
        /// Validates the data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected abstract Task ValidateDataAsync(TCommand request, CancellationToken ct);
    }
}
