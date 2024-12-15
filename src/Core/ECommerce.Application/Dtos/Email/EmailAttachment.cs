namespace ECommerce.Application.Dtos.Email;

public sealed record EmailAttachment(
    string FileName,
    byte[] Content,
    string ContentType
);