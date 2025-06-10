namespace Core.Infrastructure.Results
{
    /// <summary>
	/// The DataResult interface
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public interface IDataResult<TData> : IResult
    {
        /// <summary>
        /// The data
        /// </summary>
        TData Data { get; set; }

        /// <summary>
        /// The data query
        /// </summary>
        IQueryable<TData> Query { get; set; }
    }
}