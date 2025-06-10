using Contracts;
using Core.Dto;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Configuration;
using NotificationService.Service;
using System.Text;

namespace NotificationService.Consumers
{
    public class ConfirmnAccountConsumer : IConsumer<ConfirmnAccountNotification>
    {
        private readonly ISmtpService _smtpService;
        private readonly ClientOptions _clientOptions;

        public ConfirmnAccountConsumer(ISmtpService smtpService, IOptions<ClientOptions> clientOptions)
        {
            _smtpService = smtpService ?? throw new ArgumentNullException(nameof(smtpService));
            _clientOptions = clientOptions.Value ?? throw new ArgumentNullException(nameof(clientOptions));
        }

        public async Task Consume(ConsumeContext<ConfirmnAccountNotification> context)
        {
            var notification = context.Message;

            var textBody = new StringBuilder();
            textBody.AppendLine("Odkaz na potvrzení účtu do portálu");
            textBody.AppendLine($"{_clientOptions.Url}/register?token={notification.Identifier}");
            textBody.AppendLine("Platnost odkazu je 15 minut.");
            textBody.AppendLine();
            textBody.AppendLine($"Pokud jste se neregistroval/a vy, obraťte se na podporu.");

            var email = new EmailDto(new() { notification.Email }, new(), "Potvrzení účtu", textBody.ToString());
            await _smtpService.SendAsync(email);
        }
    }
}
