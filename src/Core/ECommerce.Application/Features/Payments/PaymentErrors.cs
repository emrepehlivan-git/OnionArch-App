using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Payments;

public static class PaymentErrors
{
    public static readonly Error PaymentFailed = new("Payment.PaymentFailed", "Payment failed");
    public static readonly Error OrderNotFound = new("Payment.OrderNotFound", "Order not found");
    public static readonly Error CardNumberRequired = new("Payment.CardNumberRequired", "Card number is required");
    public static readonly Error CardHolderNameRequired = new("Payment.CardHolderNameRequired", "Card holder name is required");
    public static readonly Error ExpiryDateRequired = new("Payment.ExpiryDateRequired", "Expiry date is required");
    public static readonly Error SecurityCodeRequired = new("Payment.SecurityCodeRequired", "Security code is required");
    public static readonly Error AmountMustBeGreaterThanZero = new("Payment.AmountMustBeGreaterThanZero", "Amount must be greater than zero");
}
