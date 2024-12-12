using ECommerce.Application.Interfaces.Services;
using MediatR;

namespace ECommerce.Application.Behaviors.Cache;

public sealed class CacheBehavior<TRequest, TResponse>(ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableRequest
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cachedValue = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
        if (cachedValue is not null)
            return cachedValue;
        var result = await next();
        await _cacheService.SetAsync(request.CacheKey, result, request.CacheExpirationTime, cancellationToken);
        return result;
    }
}
