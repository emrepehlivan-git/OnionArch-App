namespace ECommerce.Application.Dtos.Email;

public sealed record EmailRequest
{
    public string To { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public bool IsHtml { get; init; } = false;
    public string? From { get; init; }
    public string? DisplayName { get; init; }
    public List<string>? Cc { get; init; }
    public List<string>? Bcc { get; init; }
    public List<EmailAttachment>? Attachments { get; init; }
}
