using System.Net;

namespace Core.Infrastructure.Results
{
    /// <summary>
	/// The Api result interface
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public interface IApiResult<TData> : IDataResult<TData>
    {
        /// <summary>
        /// The API error code
        /// </summary>
        int StatusCode { get; set; }
    }
}
