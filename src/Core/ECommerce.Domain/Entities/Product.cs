namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; }

    private Product() { }

    private Product(string name, string description, decimal price, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }

    public static Product Create(string name, string description, decimal price, Guid categoryId)
    {
        return new Product(name, description, price, categoryId);
    }

    public void Update(string name, string description, decimal price, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }
}
