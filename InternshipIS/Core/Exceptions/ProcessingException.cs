namespace Core.Exceptions
{
    /// <summary>
    /// Exception which occurs during processing but is not InternalServerError
    /// </summary>
    public class ProcessingException : Exception
    {
        /// <summary>
        /// .ctor for ProcessingException
        /// </summary>
        /// <param name="messages"></param>
        public ProcessingException(IList<string> messages) : base(string.Join(";", messages)) { }

        /// <summary>
        /// .ctor for ProcessingException
        /// </summary>
        /// <param name="message"></param>
        public ProcessingException(string message) : base(message) { }
    }
}
