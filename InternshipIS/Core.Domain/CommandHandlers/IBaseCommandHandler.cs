using Core.Domain.Requests;
using Core.Domain.Responses;
using Core.Infrastructure.Results;
using MediatR;

namespace Core.Domain.CommandHandlers
{
    /// <summary>
    /// The interface for base command handler
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IBaseCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, IApiResult<TResponse>>
        where TResponse : IBaseResponse
        where TCommand : IBaseRequest<TResponse>
    {
    }
}
