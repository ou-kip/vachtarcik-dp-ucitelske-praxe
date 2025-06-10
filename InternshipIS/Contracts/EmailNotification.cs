namespace Contracts
{
    public class EmailNotification : IEmailNotification
    {
        public List<string> Receivers { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
