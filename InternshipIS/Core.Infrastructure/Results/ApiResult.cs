namespace Core.Infrastructure.Results
{
    /// <summary>
    /// The API result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ApiResult<TData> : DataResult<TData>, IApiResult<TData>
        where TData : new()
    {
        /// <summary>
        /// .ctor for ApiResult
        /// </summary>
        public ApiResult() 
        {
            Data = new TData();
        }
        ///<inheritdoc/>
        public int StatusCode { get; set; } = 200;
    }
}
