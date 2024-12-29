using ECommerce.Application.Interfaces.Data;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.EFCore.Contexts;
using ECommerce.EFCore.Data;
using ECommerce.EFCore.Interceptors;
using ECommerce.EFCore.Repositories;
using ECommerce.EFCore.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.EFCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEFCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(
                new AuditableEntitySaveChangesInterceptor(services.BuildServiceProvider().GetRequiredService<ICurrentUserService>()),
                new DomainEventSaveChangesInterceptor(services.BuildServiceProvider().GetRequiredService<IPublisher>()));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IStockService, StockService>();
        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        return services;
    }
}
