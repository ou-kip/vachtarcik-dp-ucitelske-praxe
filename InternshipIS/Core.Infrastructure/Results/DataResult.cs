
namespace Core.Infrastructure.Results
{
    /// <summary>
    /// The DataResult
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class DataResult<TData> : Result, IDataResult<TData>
    {
        /// <inheritdoc/>
        public TData Data { get; set; }

        /// <inheritdoc/>
        public IQueryable<TData> Query { get; set; }
    }
}
