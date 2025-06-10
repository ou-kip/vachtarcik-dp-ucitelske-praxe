namespace NotificationService.Configuration
{
    public class EmailingOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string AdminAddress { get; set; }
        public bool EnableSSL { get; set; } = true;
    }
}
