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
            .HasColumnType("decimal(18, 2)")
            .IsRequired();
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.PaymentStatus).IsRequired();
        builder.Property(o => o.PaymentMethod).IsRequired();

        builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId);
        builder.OwnsOne(o => o.Address);

        builder.HasIndex(o => o.OrderNumber).IsUnique();
    }
}
