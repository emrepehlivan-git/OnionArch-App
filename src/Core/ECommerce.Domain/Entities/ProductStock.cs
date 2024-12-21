namespace ECommerce.Domain.Entities;

public sealed class ProductStock : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int Stock { get; private set; }

    private ProductStock() { }
    private ProductStock(Guid productId, int stock)
    {
        ProductId = productId;
        Stock = stock;
    }

    public static ProductStock Create(Guid productId, int stock)
    {
        return new(productId, stock);
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
