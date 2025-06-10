using Core.Domain.Responses;
using Core.Infrastructure.Results;
using MediatR;

namespace Core.Domain.Requests
{
    /// <summary>
    /// the interface for base request for command or query
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IBaseRequest<TResponse> : IRequest<IApiResult<TResponse>>
        where TResponse : IBaseResponse
    {
    }
}
