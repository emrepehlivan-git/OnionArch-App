namespace ECommerce.Application.Behaviors.Cache;

public interface ICacheableRequest
{
    string CacheKey { get; }
    TimeSpan CacheExpirationTime { get; }
}
