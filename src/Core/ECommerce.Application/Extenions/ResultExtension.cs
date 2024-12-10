using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Extenions;

public static class ResultExtension
{
    /// <summary>
    /// Ensures success and returns the value, otherwise throws an exception
    /// </summary>
    public static T EnsureSuccess<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return result.Value!;

        throw new InvalidOperationException($"Operation failed: {result.Error}");
    }

    /// <summary>
    /// Maps a successful result to a new value
    /// </summary>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
    {
        if (!result.IsSuccess)
            return Result<TOut>.Failure(result.Error);

        return Result<TOut>.Success(mapper(result.Value!));
    }

    /// <summary>
    /// Binds a result to another async operation
    /// </summary>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func)
    {
        if (!result.IsSuccess)
            return Result<TOut>.Failure(result.Error);

        return await func(result.Value!);
    }

    /// <summary>
    /// Matches the result to either success or failure case
    /// </summary>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> success,
        Func<Error, TOut> failure)
    {
        return result.IsSuccess ? success(result.Value!) : failure(result.Error);
    }

    /// <summary>
    /// Matches the result to either success or failure case
    /// </summary>
    public static TOut Match<TOut>(this Result result, Func<TOut> success, Func<Error, TOut> failure)
    {
        return result.IsSuccess ? success() : failure(result.Error);
    }
}
