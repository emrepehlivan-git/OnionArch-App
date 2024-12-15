using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Payment : BaseEntity
{
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }

    private Payment() { }

    private Payment(decimal amount, Guid orderId)
    {
        Amount = amount;
        PaymentDate = DateTime.UtcNow;
        Status = PaymentStatus.Pending;
        OrderId = orderId;
    }

    public static Payment Create(decimal amount, Guid orderId)
    {
        return new(amount, orderId);
    }

    public void UpdatePaymentStatus(PaymentStatus status)
    {
        Status = status;
    }
}