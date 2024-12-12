using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount => OrderItems.Sum(item => item.Price * item.Quantity);
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public virtual ICollection<OrderItem> OrderItems { get; private set; } = [];

    private Order() { }

    private Order(string orderNumber,
        DateTime orderDate,
        OrderStatus status,
        PaymentStatus paymentStatus,
        PaymentMethod paymentMethod)
    {
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        Status = status;
        PaymentStatus = paymentStatus;
        PaymentMethod = paymentMethod;
    }

    public static Order Create(
        string orderNumber,
        DateTime orderDate,
        OrderStatus status,
        PaymentStatus paymentStatus,
        PaymentMethod paymentMethod)
    {
        return new(orderNumber, orderDate, status, paymentStatus, paymentMethod);
    }

    public void AddItem(OrderItem item)
    {
        OrderItems.Add(item);
    }
}
