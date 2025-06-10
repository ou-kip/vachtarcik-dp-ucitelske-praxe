namespace Core.Exceptions
{
    /// <summary>
    /// The exception during signing in
    /// </summary>
    public class SignInException : Exception
    {
        /// <summary>
        /// .ctor for SignInException
        /// </summary>
        /// <param name="email"></param>
        public SignInException(string email = "") : base($"Unable to sign user: {email}") 
        {
        }

        /// <summary>
        /// .ctor for SignInException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="email"></param>
        public SignInException(string message = "", string email = "") : base($"{message}, {email}")
        {
        }
    }
}
