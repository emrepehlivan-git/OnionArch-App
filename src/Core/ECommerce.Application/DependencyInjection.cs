using System.Reflection;
using ECommerce.Application.Behaviors.Cache;
using ECommerce.Application.Behaviors.Transaction;
using ECommerce.Application.Behaviors.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
        });
        return services;
    }
}
