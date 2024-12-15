namespace ECommerce.Application.Dtos.Email;

public sealed record EmailRequest(
    string To,
    string Subject,
    string Body,
    bool IsHtml = false,
    string? From = null,
    string? DisplayName = null,
    List<string>? Cc = null,
    List<string>? Bcc = null,
    List<EmailAttachment>? Attachments = null
);