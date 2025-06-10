using System.Net;

namespace Core.Domain.Responses
{
    /// <summary>
    /// The base command response
    /// </summary>
    public class BaseResponse : IBaseResponse
    {
        public int StatusCode {  get; set; }
        public string Message {  get; set; }
    }
}
