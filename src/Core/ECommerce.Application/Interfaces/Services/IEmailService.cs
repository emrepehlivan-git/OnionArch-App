using ECommerce.Application.Dtos.Email;

namespace ECommerce.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default);
    Task SendEmailAsync(IEnumerable<EmailRequest> requests, CancellationToken cancellationToken = default);
}
