using ECommerce.Application.Interfaces.Services;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
        });

        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}
