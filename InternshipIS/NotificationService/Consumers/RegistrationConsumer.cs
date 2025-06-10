using Contracts;
using Core.Dto;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Configuration;
using NotificationService.Service;
using System.Text;

namespace NotificationService.Consumers
{
    public class RegistrationConsumer : IConsumer<UserRegisteredNotification>
    {
        private readonly ISmtpService _smtpService;
        private readonly ClientOptions _clientOptions;

        public RegistrationConsumer(ISmtpService smtpService, IOptions<ClientOptions> clientOptions)
        {
            _smtpService = smtpService ?? throw new ArgumentNullException(nameof(smtpService));
            _clientOptions = clientOptions.Value ?? throw new ArgumentNullException(nameof(clientOptions));
        }

        public async Task Consume(ConsumeContext<UserRegisteredNotification> context)
        {
            var notification = context.Message;

            var builder = new StringBuilder();
            builder.AppendLine("Byl/a jste zaregistrován/a do systému.");
            builder.AppendLine($"Odkaz pro přihlášení: {_clientOptions.Url}");

            var subject = "Portal pro zadávání praxí - registrace dokončena";

            var email = new EmailDto(new() { context.Message.Email }, new(), subject, builder.ToString());

            await _smtpService.SendAsync(email);
            //TODO CREATE HTML TEMPLATE
        }
    }
}
