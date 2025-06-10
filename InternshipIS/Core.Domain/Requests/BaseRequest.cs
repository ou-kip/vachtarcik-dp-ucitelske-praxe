using Core.Domain.Responses;
using System.Text.Json.Serialization;

namespace Core.Domain.Requests
{
    /// <summary>
    /// The base request for command or handler
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class BaseRequest<TResponse> : IBaseRequest<TResponse>
        where TResponse : IBaseResponse
    {
        [JsonIgnore]
        public string? PerformerEmail { get; internal set; }

        /// <summary>
        /// Sets the value of PerformerEmail
        /// </summary>
        /// <param name="email"></param>
        public void SetPerformer(string? email)
        {
            PerformerEmail = email;
        }
    }
}
