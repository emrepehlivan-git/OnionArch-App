using ECommerce.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Extensions
{
    public static class ResultExtension
    {
        /// <summary>
        /// Maps the successful Result to a new Result using the provided function.
        /// </summary>
        public static Result<TDestination> Map<TDestination>(
            this Result<TDestination> result,
            Func<TDestination> func)
        {
            return result.IsSuccess ? Result<TDestination>.Success(func()) : Result<TDestination>.Failure(result.Error);
        }

        /// <summary>
        /// Maps the successful Result<TSource> to Result<TDestination> using the provided function.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="func">Function</param>
        /// <returns>Result<TDestination></returns>
        public static Result<TDestination> Map<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, TDestination> func)
        {
            return result.IsSuccess ? Result<TDestination>.Success(func(result.Value!)) : Result<TDestination>.Failure(result.Error);
        }

        /// <summary>
        /// Maps the Result to an IActionResult using the provided success and failure functions.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="onSuccess">Success function</param>
        /// <param name="onFailure">Failure function</param>
        /// <returns>IActionResult</returns>
        public static IActionResult Map<T>(
            this Result<T> result,
            Func<T, IActionResult> onSuccess,
            Func<Error, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value!) : onFailure(result.Error);
        }

        /// <summary>
        /// Maps the Result to a new Result using the provided function.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="func">Function</param>
        /// <returns>Result</returns>
        public static Result Map(
            this Result result,
            Func<Result> func)
        {
            return result.IsSuccess ? func() : Result.Failure(result.Error);
        }

        /// <summary>
        /// Maps the Result to an IActionResult using the provided function for both success and failure.
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="func">Function</param>
        /// <returns>IActionResult</returns>
        public static IActionResult Map(
            this Result result,
            Func<Result, IActionResult> func)
        {
            return result.IsSuccess ? func(result) : func(Result.Failure(result.Error));
        }

        /// <summary>
        /// Maps the successful Result<TSource> to Result using the provided function.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="func">Function</param>
        /// <returns>Result</returns>
        public static Result Map<TSource>(
            this Result<TSource> result,
            Func<TSource, Result> func)
        {
            return result.IsSuccess ? func(result.Value!) : Result.Failure(result.Error);
        }

        /// <summary>
        /// Asynchronously maps the successful Result<TSource> to Result<TDestination>.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="result">Result to map</param>
        /// <param name="asyncFunc">Async function</param>
        /// <returns>Result<TDestination></returns>
        public static async Task<Result<TDestination>> MapAsync<TSource, TDestination>(
            this Result<TSource> result,
            Func<TSource, Task<TDestination>> asyncFunc)
        {
            if (result.IsSuccess)
            {
                var destination = await asyncFunc(result.Value!);
                return Result<TDestination>.Success(destination);
            }
            else
            {
                return Result<TDestination>.Failure(result.Error);
            }
        }

        /// <summary>
        /// Binds the successful Result<TSource> to Result<TDestination> using the provided binder function.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="result">Result to bind</param>
        /// <param name="binder">Binder function</param>
        /// <returns>Result<TDestination></returns>
        public static Result<TDestination> Bind<TSource, TDestination>(
            this Result<TSource> result,
            Func<Result<TSource>, Result<TDestination>> binder)
        {
            return binder(result);
        }

        /// <summary>
        /// Asynchronously binds the successful Result<TSource> to Result<TDestination>.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="result">Result to bind</param>
        /// <param name="asyncBinder">Async binder function</param>
        /// <returns>Result<TDestination></returns>
        public static async Task<Result<TDestination>> BindAsync<TSource, TDestination>(
            this Result<TSource> result,
            Func<Result<TSource>, Task<Result<TDestination>>> asyncBinder)
        {
            return await asyncBinder(result);
        }
    }
}