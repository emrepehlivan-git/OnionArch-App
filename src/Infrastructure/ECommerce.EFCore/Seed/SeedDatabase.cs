using Bogus;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;
using ECommerce.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.EFCore.Seed;

public sealed class SeedDatabase(ECommerceDbContext context)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await context.Database.EnsureCreatedAsync(cancellationToken);
        List<Category> categories = new();
        List<Product> products = new();

        Randomizer.Seed = new Random(42);

        if (!await context.Categories.AnyAsync(cancellationToken))
        {
            categories = GenerateCategories(5);
            context.Categories.AddRange(categories);
        }

        if (!await context.Products.AnyAsync(cancellationToken))
        {
            products = GenerateProducts(categories, 20);
            context.Products.AddRange(products);
        }

        if (!await context.Orders.AnyAsync(cancellationToken))
        {
            var orders = GenerateOrders(products, 10);
            context.Orders.AddRange(orders);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private List<Category> GenerateCategories(int count)
    {
        var categories = new List<Category>()
        {
            Category.Create("Electronics"),
            Category.Create("Clothing"),
            Category.Create("Home & Kitchen"),
            Category.Create("Books"),
            Category.Create("Sports & Outdoors"),
            Category.Create("Toys & Games"),
            Category.Create("Automotive"),
            Category.Create("Health & Beauty"),
            Category.Create("Jewelry"),
            Category.Create("Pet Supplies")
        };

        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.PickRandom(categories.Select(c => c.Name)))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past())
            .RuleFor(c => c.CreatedBy, f => Guid.NewGuid());

        return categoryFaker.Generate(count).ToList();
    }

    private List<Product> GenerateProducts(List<Category> categories, int count)
    {
        var productFaker = new Faker<Product>()
            .CustomInstantiator(f =>
            {
                var category = f.PickRandom(categories);
                return Product.Create(
                    f.Lorem.Sentance(3, 5),
                    f.Lorem.Sentance(10, 20),
                    f.Finance.Amount(10, 500),
                    category.Id,
                    f.Random.Number(0, 100)
                );
            })
            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
            .RuleFor(p => p.CreatedBy, f => Guid.NewGuid());

        return productFaker.Generate(count).ToList();
    }

    private List<Order> GenerateOrders(List<Product> products, int count)
    {
        var orderFaker = new Faker<Order>()
            .CustomInstantiator(f =>
            {
                var paymentMethod = f.PickRandom<PaymentMethod>();
                var address = new Address(
                    f.Address.StreetAddress(),
                    f.Address.City(),
                    f.Address.State(),
                    f.Address.ZipCode(),
                    f.Address.Country()
                );

                var order = Order.Create(paymentMethod, address);

                // Add 1-3 random order items
                var orderItemCount = f.Random.Number(1, 3);
                var orderItems = Enumerable.Range(0, orderItemCount)
                    .Select(_ =>
                    {
                        var product = f.PickRandom(products);
                        var quantity = f.Random.Number(1, 5);
                        return OrderItem.Create(product.Id, order.Id, product.Price, quantity);
                    })
                    .ToList();

                order.AddItems(orderItems);
                order.AddPayment(orderItems.Sum(oi => oi.TotalPrice));

                return order;
            })
            .RuleFor(o => o.CreatedAt, f => f.Date.Past())
            .RuleFor(o => o.CreatedBy, f => Guid.NewGuid());

        return orderFaker.Generate(count).ToList();
    }
}
