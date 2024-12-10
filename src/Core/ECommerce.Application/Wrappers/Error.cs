namespace ECommerce.Application.Wrappers;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Null = new("Error.Null", "The specified result value is null.");
    public static Error Invalid(string propertyName, string errorMessage) => new($"Invalid.{propertyName}", errorMessage);

    public string Code { get; private set; }
    public string Message { get; private set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static implicit operator string(Error error) => error.Code;
    public static implicit operator Error(string code) => new(code, string.Empty);
}
