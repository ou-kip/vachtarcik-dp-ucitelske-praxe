namespace Core.Messaging.Configuration
{
    /// <summary>
    /// The options for messaging e.g. message broker - rabbitmq
    /// </summary>
    public class MessagingOptions
    {
        public string Host {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
