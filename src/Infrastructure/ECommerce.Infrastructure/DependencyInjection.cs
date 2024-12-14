using ECommerce.Application.Interfaces.Services;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
        });

        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<ICacheService, CacheSerivce>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}
