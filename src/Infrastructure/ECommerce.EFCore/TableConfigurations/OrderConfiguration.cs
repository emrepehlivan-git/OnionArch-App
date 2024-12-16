using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.EFCore.TableConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber).IsRequired();
        builder.Property(o => o.OrderDate).IsRequired();
        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18, 2)");
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.PaymentMethod).IsRequired();

        // Address için OwnsOne kullanımı
        builder.OwnsOne(o => o.Address, a =>
        {
            a.Property(p => p.Country).HasColumnName("Country").IsRequired();
            a.Property(p => p.Street).HasColumnName("Street").IsRequired();
            a.Property(p => p.City).HasColumnName("City").IsRequired();
            a.Property(p => p.State).HasColumnName("State").IsRequired();
            a.Property(p => p.ZipCode).HasColumnName("ZipCode").IsRequired();
        });

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        builder.HasIndex(o => o.OrderNumber).IsUnique();
    }
}
