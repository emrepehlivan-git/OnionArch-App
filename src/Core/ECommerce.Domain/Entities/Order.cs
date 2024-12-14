using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public Address Address { get; private set; }
    public decimal TotalAmount => OrderItems.Sum(item => item.Price * item.Quantity);
    public PaymentMethod PaymentMethod { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    public virtual ICollection<OrderItem> OrderItems { get; private set; } = [];

    private Order() { }

    private Order(PaymentMethod paymentMethod, Address address)
    {
        OrderNumber = GenerateOrderNumber();
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        PaymentStatus = PaymentStatus.Pending;
        PaymentMethod = paymentMethod;
        Address = address;
    }

    private string GenerateOrderNumber()
    {
        string timestamp = DateTime.UtcNow.Ticks.ToString("X");

        string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string randomPart = new string(Enumerable.Repeat(allowedChars, 6)
            .Select(s => s[Random.Shared.Next(s.Length)])
            .ToArray());

        return $"ORD-{timestamp}-{randomPart}";
    }

    public static Order Create(PaymentMethod paymentMethod, Address address)
    {
        return new Order(paymentMethod, address);
    }

    public void AddItems(IEnumerable<OrderItem> items)
    {
        foreach (var item in items)
        {
            OrderItems.Add(item);
        }
    }

    public void RemoveItem(OrderItem item)
    {
        OrderItems.Remove(item);
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}
