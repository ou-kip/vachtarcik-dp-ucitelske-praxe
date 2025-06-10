namespace Core.Dto
{
    /// <summary>
    /// the dto representing an email message
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// .ctor for EmailDto
        /// </summary>
        /// <param name="receivers">A list containing one or many receivers(required)</param>
        /// <param name="attachments">A list with or without attachments(optional). Attachment represents a string path to the file with file name and ext.</param>
        /// <param name="subject">The subject of the message(required)</param>
        /// <param name="textBody">The text body of the message(optional)</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EmailDto(List<string> receivers, List<string> attachments, string subject, string textBody = "")
        {
            Receivers = receivers ?? throw new ArgumentNullException(nameof(receivers));
            Subject = !string.IsNullOrEmpty(subject) ? Subject = subject : throw new ArgumentNullException(subject);
            Attachments = attachments;
            TextBody = textBody;
        }

        /// <summary>
        /// List with one or many receivers
        /// </summary>
        public List<string> Receivers { get; }

        /// <summary>
        /// List with one or many attachments
        /// </summary>
        public List<string> Attachments { get; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// The text body of the message
        /// </summary>
        public string TextBody { get; }

        /// <summary>
        /// Flag indicating whether the email contains attachments
        /// </summary>
        public bool HasAttachments { get { return Attachments != null && Attachments.Any() ? true : false; } }
    }
}
