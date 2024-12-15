using ECommerce.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Extenions;

public static class ResultExtension
{
    /// <summary>
    /// Maps the result to either success or failure case
    /// </summary>
    public static Result<TDestination> Map<TDestination>(
        this Result<TDestination> result,
        Func<TDestination> func)
    {
        return result.IsSuccess ? Result<TDestination>.Success(func()) : Result<TDestination>.Failure(result.Error);
    }

    /// <summary>
    /// Maps the result to either success or failure case
    /// </summary>
    public static Result<TDestination> Map<TSource, TDestination>(
        this Result<TSource> result,
        Func<TSource, TDestination> func)
    {
        return result.IsSuccess ? Result<TDestination>.Success(func(result.Value!)) : Result<TDestination>.Failure(result.Error);
    }

    /// <summary>
    /// Maps a Result<T> to an IActionResult
    /// </summary>
    public static IActionResult Map<T>(
        this Result<T> result,
        Func<T, IActionResult> onSuccess,
        Func<Error, IActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value!) : onFailure(result.Error);
    }

    /// <summary>
    /// Maps a Result to another Result
    /// </summary>
    public static Result Map(
        this Result result,
        Func<Result> func)
    {
        return result.IsSuccess ? func() : Result.Failure(result.Error);
    }

    public static IActionResult Map(
        this Result result,
        Func<Result, IActionResult> func)
    {
        return result.IsSuccess ? func(result) : func(Result.Failure(result.Error));
    }

    /// <summary>
    /// Maps a Result<TSource> to another Result
    /// </summary>
    public static Result Map<TSource>(
        this Result<TSource> result,
        Func<TSource, Result> func)
    {
        return result.IsSuccess ? func(result.Value!) : Result.Failure(result.Error);
    }
}
