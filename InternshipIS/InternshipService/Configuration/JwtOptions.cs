namespace InternshipService.Configuration
{
    /// <summary>
    /// The options for Jwt authorization
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// The key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The subject
        /// </summary>
        public string Subject { get; set; }
    }
}
