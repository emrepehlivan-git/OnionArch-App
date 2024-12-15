namespace ECommerce.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public Order Order { get; private set; }
    public Guid OrderId { get; private set; }
    public Product Product { get; private set; }
    public Guid ProductId { get; private set; }

    public decimal TotalPrice { get; private set; }

    private OrderItem() { }

    private OrderItem(Guid productId, Guid orderId, decimal price, int quantity)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        OrderId = orderId;
        TotalPrice = price * quantity;
    }

    public static OrderItem Create(Guid productId, Guid orderId, decimal price, int quantity)
    {
        return new(productId, orderId, price, quantity);
    }
}
