namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; }

    private Product() { }

    private Product(string name, string description, decimal price, Guid categoryId, int stock)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
    }

    public static Product Create(string name, string description, decimal price, Guid categoryId, int stock)
        => new Product(name, description, price, categoryId, stock);

    public void Update(string name, string description, decimal price, Guid categoryId, int stock)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
    }

    public void DecreaseStock(int quantity)
    {
        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
    }
}
