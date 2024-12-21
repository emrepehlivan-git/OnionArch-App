using System.Reflection;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.EFCore.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.EFCore.Contexts;

public class ECommerceDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options, IServiceProvider serviceProvider)
        : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<Payment> Payments { get; set; } = null!;
    public virtual DbSet<ProductStock> ProductStocks { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.AddInterceptors(GetSaveChangesInterceptors());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private IInterceptor[] GetSaveChangesInterceptors()
    {
        var currentUserService = _serviceProvider.GetRequiredService<ICurrentUserService>();
        var publisher = _serviceProvider.GetRequiredService<IPublisher>();
        return
        [
            new AuditableEntitySaveChangesInterceptor(currentUserService),
            new DomainEventSaveChangesInterceptor(publisher)
        ];
    }
}
