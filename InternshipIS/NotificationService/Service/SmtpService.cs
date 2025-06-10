using Core.Constants;
using Core.Dto;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Configuration;

namespace NotificationService.Service
{
    /// <summary>
    /// The smtp service for sending the emails
    /// </summary>
    public class SmtpService : ISmtpService
    {
        /// <summary>
        /// The emailing options
        /// </summary>
        private readonly EmailingOptions _options;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor for SmtpService
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SmtpService(IOptions<EmailingOptions> options, ILogger<SmtpService> logger) 
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        /// <summary>
        /// Sends the email message via smtp client
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task SendAsync(EmailDto emailMessage, CancellationToken ct = default)
        {
            if(emailMessage == null) { throw new ArgumentNullException(nameof(emailMessage)); }

            //todo null checks before calling the client
            await SendInternalAsync(emailMessage, ct);
        }

        /// <summary>
        /// Sends internal the email, only wrapper method
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task SendInternalAsync(EmailDto emailMessage, CancellationToken ct = default)
        {
            try
            {
                //prepare objects
                var email = new MimeMessage();
                var body = new BodyBuilder();

                //set body content + attachments
                body.TextBody = emailMessage.TextBody;

                //add the attachments
                if (emailMessage.HasAttachments) { emailMessage.Attachments.ForEach(x => body.Attachments.Add(x, ct)); }              

                //set the email parts
                email.From.Add(MailboxAddress.Parse(_options.Email));
                email.Subject = emailMessage.Subject;
                email.Body = body.ToMessageBody();

                //add receivers
                emailMessage.Receivers.ForEach(x => email.To.Add(MailboxAddress.Parse(CheckAdminEmail(x))));

                //prepare the client with or without tls/ssl
                var client = new MailKit.Net.Smtp.SmtpClient();
                if (_options.EnableSSL)
                {
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
                    client.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
                }
                else
                {
                    client.Connect(_options.Host, _options.Port, SecureSocketOptions.None);
                }               
                
                client.Authenticate(_options.Email, _options.Password);

                _logger.LogInformation($"Sending email '{emailMessage.Subject}' via smtp.");

                await client.SendAsync(email);
                client.Disconnect(true);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Checks whether the email address should be admins address from options
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private string CheckAdminEmail(string email)
        {
            if (email.Equals(EmailConstants.Admin))
            {
                return _options.AdminAddress;
            }

            else return email;
        }
    }
}
