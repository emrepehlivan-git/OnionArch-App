using ECommerce.Application;
using ECommerce.Application.Settings;
using ECommerce.EFCore;
using ECommerce.EFCore.Contexts;
using ECommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ECommerce.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApplicationServices()
            .AddInfrastructureServices()
            .AddEFCoreServices(configuration)
            .ConfigureSettings(configuration)
            .AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger());

        return services;
    }

    public static WebApplication UseWebApiServices(this WebApplication app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            await dbContext.Database.MigrateAsync();
    }

    private static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));
        return services;
    }
}
