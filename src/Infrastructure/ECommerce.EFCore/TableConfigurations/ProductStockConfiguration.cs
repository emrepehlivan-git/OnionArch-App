using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.EFCore.TableConfigurations;

public sealed class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("product_stock");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.ProductId).IsRequired();
        builder.Property(ps => ps.Stock).IsRequired();

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<ProductStock>(ps => ps.ProductId);

        builder.HasIndex(ps => ps.ProductId).IsUnique();
    }
}
