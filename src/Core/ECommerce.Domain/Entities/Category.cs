namespace ECommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }

    public virtual ICollection<Product> Products { get; private set; } = [];

    private Category()
    {
        Name = string.Empty;
    }

    private Category(string name)
    {
        Name = name;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }

    public void Update(string name)
    {
        Name = name;
    }
}
