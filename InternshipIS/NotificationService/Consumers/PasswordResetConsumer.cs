using Contracts;
using Core.Dto;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Configuration;
using NotificationService.Service;
using System.Text;

namespace NotificationService.Consumers
{
    public class PasswordResetConsumer : IConsumer<ResetPasswordNotification>
    {
        private readonly ISmtpService _smtpService;
        private readonly ClientOptions _clientOptions;

        public PasswordResetConsumer(ISmtpService smtpService, IOptions<ClientOptions> clientOptions)
        {
            _smtpService = smtpService ?? throw new ArgumentNullException(nameof(smtpService));
            _clientOptions = clientOptions.Value ?? throw new ArgumentNullException(nameof(clientOptions));
        }

        public async Task Consume(ConsumeContext<ResetPasswordNotification> context)
        {
            var notification = context.Message;

            var textBody = new StringBuilder();
            textBody.AppendLine("Odkaz na změnu hesla do portálu pro zadávání praxí");
            textBody.AppendLine($"{_clientOptions.Url}/change-password?token={notification.Identifier}");
            textBody.AppendLine();
            textBody.AppendLine($"Pokud jste o změnu hesla nepožádal/a, email ignorujte.");

            var email = new EmailDto(new() { notification.Email }, new(), "Změna hesla", textBody.ToString());
            await _smtpService.SendAsync(email);
        }
    }
}
