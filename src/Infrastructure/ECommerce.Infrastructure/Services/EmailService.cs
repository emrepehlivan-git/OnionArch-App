using System.Net.Mail;
using ECommerce.Application.Dtos.Email;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Settings;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Services;

public sealed class EmailService(IOptions<MailSettings> mailSettings) : IEmailService
{
    private readonly MailSettings _mailSettings = mailSettings.Value;

    public async Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var smtpClient = GetSmtpClient();
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_mailSettings.From, _mailSettings.DisplayName),
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = request.IsHtml
        };

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }

    public async Task SendEmailAsync(IEnumerable<EmailRequest> requests, CancellationToken cancellationToken = default)
    {
        foreach (var request in requests)
        {
            await SendEmailAsync(request, cancellationToken);
        }
    }

    private SmtpClient GetSmtpClient() => new(_mailSettings.Host, _mailSettings.Port)
    {
        EnableSsl = _mailSettings.EnableSsl,
    };
}
