using ECommerce.Domain.DomainEvents.Orders;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class Order : BaseEntity, IAuditableEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public Address Address { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Payment Payment { get; private set; }
    public ICollection<OrderItem> OrderItems { get; private set; } = [];

    private Order() { }

    private Order(PaymentMethod paymentMethod, Address address)
    {
        OrderNumber = GenerateOrderNumber();
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
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
        return new(paymentMethod, address);
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

    public void UpdateOrderStatus(OrderStatus status)
    {
        Status = status;
    }

    public void AddPayment(decimal amount)
    {
        Payment = Payment.Create(amount, Id);
    }

    public void UpdatePaymentStatus(PaymentStatus status)
    {
        Payment?.UpdatePaymentStatus(status);
    }

    public PaymentStatus GetPaymentStatus()
    {
        return Payment?.Status ?? PaymentStatus.Pending;
    }
}
