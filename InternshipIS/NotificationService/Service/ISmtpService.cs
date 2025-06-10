using Core.Dto;

namespace NotificationService.Service
{
    /// <summary>
    /// The interface for SmtpService
    /// </summary>
    public interface ISmtpService
    {
        /// <summary>
        /// Sends the email message via smtp client
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task SendAsync(EmailDto emailMessage, CancellationToken ct = default);
    }
}
