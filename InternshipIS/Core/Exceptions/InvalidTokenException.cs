namespace Core.Exceptions
{
    /// <summary>
    /// The exception when token value is not valid
    /// </summary>
    public class InvalidTokenException : Exception
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="value"></param>
        public InvalidTokenException(string value) : base($"The provided token is not valid: '{value}'")
        {
            
        }
    }
}
