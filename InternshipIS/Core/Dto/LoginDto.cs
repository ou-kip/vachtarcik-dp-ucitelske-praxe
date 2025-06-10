namespace Core.Dto
{
    /// <summary>
    /// Data transfer object with login informations
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// .ctor for LoginDto
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public LoginDto(string email, string password, string? token = null)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(email));
            Token = token;
        }

        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The token
        /// </summary>
        public string? Token { get; }
    }
}
