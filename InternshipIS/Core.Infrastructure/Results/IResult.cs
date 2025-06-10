namespace Core.Infrastructure.Results
{
    /// <summary>
	/// The Result interface
	/// </summary>
	public interface IResult
    {
        /// <summary>
        /// Check whether Result ended with errors
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// List of errors
        /// </summary>
        IList<Exception> Errors { get; }

        /// <summary>
        /// Adds the error to the result
        /// </summary>
        /// <param name="error"></param>
        void AddError(Exception error);

        /// <summary>
        /// Merges the errors with another result errors
        /// </summary>
        /// <param name="result"></param>
        void MergeErrors(IResult result);

        /// <summary>
        /// Gets the message which contains all exception messages
        /// </summary>
        /// <returns></returns>
        string GetErrorMessage();
    }
}
