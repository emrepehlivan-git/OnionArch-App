using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.EFCore.TableConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.Price).IsRequired();
        builder.Property(oi => oi.TotalPrice).IsRequired();

        builder.HasOne<Order>().WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId);
        builder.HasOne<Product>().WithMany().HasForeignKey(oi => oi.ProductId);

        builder.HasIndex(oi => new { oi.OrderId, oi.ProductId }).IsUnique();
    }
}
