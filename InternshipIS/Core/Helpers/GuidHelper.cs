namespace Core.Helpers
{
    /// <summary>
    /// The helepr class for GUIDs
    /// </summary>
    public static class GuidHelper
    {
        /// <summary>
        /// Generates shortened value of guid
        /// </summary>
        /// <returns></returns>
        public static string GenerateShortGuid()
        {
            return Convert.ToBase64String(
                Guid.NewGuid()
                .ToByteArray())
                .Replace("==", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty)
                .Replace(" ", string.Empty);
        }
    }
}
