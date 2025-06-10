namespace Contracts
{
    public interface IEmailNotification
    {
        public List<string> Receivers { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
