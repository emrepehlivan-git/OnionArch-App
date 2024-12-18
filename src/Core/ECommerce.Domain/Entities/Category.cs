using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.Entities;

public class Category : BaseEntity, IAuditableEntity
{
    public string Name { get; private set; }

    public virtual ICollection<Product> Products { get; private set; } = [];
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

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
