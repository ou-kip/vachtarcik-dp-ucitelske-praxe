using System.Net;

namespace Core.Domain.Responses
{
    /// <summary>
    /// The interface for base command response
    /// </summary>
    public interface IBaseResponse
    {
        /// <summary>
        /// The Status code of the response
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// The message describing the response
        /// </summary>
        string Message { get; set; }
    }
}
