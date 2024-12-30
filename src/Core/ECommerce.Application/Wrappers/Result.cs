using System.Text.Json.Serialization;

namespace ECommerce.Application.Wrappers;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public IEnumerable<Error>? ValidationErrors { get; }

    [JsonConstructor]
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    protected Result(bool isSuccess, Error error, IEnumerable<Error>? validationErrors = null)
        : this(isSuccess, error)
    {
        ValidationErrors = validationErrors;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Invalid(IEnumerable<Error> validationErrors) => new(false, Error.None, validationErrors);

    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<T> : Result
{
    private readonly T _value;
    public T? Value => IsSuccess ? _value : default;

    [JsonConstructor]
    protected Result(T value, bool isSuccess, Error error, IEnumerable<Error>? validationErrors = null)
        : base(isSuccess, error, validationErrors)
    {
        _value = value;
    }

    public static Result<T> Success(T value) => new(value, true, Error.None);
    public static new Result<T> Failure(Error error) => new(default!, false, error);
    public static new Result<T> Invalid(IEnumerable<Error> validationErrors)
        => new(default!, false, Error.None, validationErrors);

    public static Result<T> From(Result result) => new(default!, result.IsSuccess, result.Error, result.ValidationErrors);
}
