namespace Core.Infrastructure.Results
{
    /// <summary>
    /// The Result
    /// </summary>
    public class Result : IResult
    {
        /// <inheritdoc/>
        public IList<Exception> Errors { get; } = new List<Exception>();

        /// <inheritdoc/>
        public bool HasErrors => Errors.Any();

        /// <inheritdoc/>
        public void AddError(Exception error) => Errors.Add(error);

        /// <inheritdoc/>
        public string GetErrorMessage() => string.Join("||", Errors.Select(x => x.Message));

        /// <inheritdoc/>
        public void MergeErrors(IResult result) => ((List<Exception>)Errors).AddRange(result.Errors);
    }
}
