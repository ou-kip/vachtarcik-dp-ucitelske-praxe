using Contracts;
using Core.Dto;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Configuration;
using NotificationService.Service;

namespace NotificationService.Consumers
{
    public class CommonEmailConsumer : IConsumer<EmailNotification>
    {
        private readonly ISmtpService _smtpService;

        public CommonEmailConsumer(ISmtpService smtpService, IOptions<ClientOptions> clientOptions)
        {
            _smtpService = smtpService ?? throw new ArgumentNullException(nameof(smtpService));
        }

        public async Task Consume(ConsumeContext<EmailNotification> context)
        {
            await ProcessAsync(context);
        }

        private async Task ProcessAsync<T>(ConsumeContext<T> context, CancellationToken ct = default)
            where T: class, IEmailNotification
        {
            var notification = context.Message;

            var emailDto = new EmailDto(notification.Receivers, new(), notification.Subject, notification.Message);
            await _smtpService.SendAsync(emailDto);
        }
    }
}
