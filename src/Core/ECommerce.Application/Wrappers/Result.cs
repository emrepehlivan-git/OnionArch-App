namespace ECommerce.Application.Wrappers;

/// <summary>
/// Represents the result of an operation, containing success status and error information.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Contains error information if the operation failed.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Contains validation errors if the operation failed due to validation issues.
    /// </summary>
    public IEnumerable<Error>? ValidationErrors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    /// <param name="error">The error information.</param>
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with validation errors.
    /// </summary>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    /// <param name="error">The error information.</param>
    /// <param name="validationErrors">The validation errors.</param>
    protected Result(bool isSuccess, Error error, IEnumerable<Error>? validationErrors = null)
        : this(isSuccess, error)
    {
        ValidationErrors = validationErrors;
    }

    /// <summary>
    /// Represents a successful result without any errors.
    /// </summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error information.</param>
    /// <returns>A failure result.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Creates an invalid result with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>An invalid result.</returns>
    public static Result Invalid(IEnumerable<Error> validationErrors) => new(false, Error.None, validationErrors);
}

/// <summary>
/// Represents the result of an operation with a value, containing success status and error information.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
public class Result<T> : Result
{
    private readonly T _value;

    /// <summary>
    /// Gets the value returned by the operation if it was successful.
    /// </summary>
    public T? Value => _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    /// <param name="error">The error information.</param>
    private Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with validation errors.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    /// <param name="error">The error information.</param>
    /// <param name="validationErrors">The validation errors.</param>
    protected Result(T value, bool isSuccess, Error error, IEnumerable<Error>? validationErrors = null)
        : base(isSuccess, error, validationErrors)
    {
        _value = value;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <returns>A successful result.</returns>
    public static Result<T> Success(T value) => new(value, true, Error.None);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error information.</param>
    /// <returns>A failure result.</returns>
    public static new Result<T> Failure(Error error) => new(default!, false, error);

    /// <summary>
    /// Creates an invalid result with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>An invalid result.</returns>
    public static new Result<T> Invalid(IEnumerable<Error> validationErrors)
        => new(default!, false, "Validation Error", validationErrors);

    /// <summary>
    /// Implicitly converts a value to a successful <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value) => Success(value);
}
