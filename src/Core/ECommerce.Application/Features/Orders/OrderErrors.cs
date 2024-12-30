using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Orders;

public static class OrderErrors
{
    public static Error OrderNotFound => new("OrderNotFound", "Order not found");
    public static Error OrderCreationFailed => new("OrderCreationFailed", "Order creation failed");
    public static Error InvalidPaymentMethod => new("InvalidPaymentMethod", "Invalid payment method");
    public static Error InvalidAddress => new("InvalidAddress", "All fields are required");
    public static Error StockNotAvailable(Guid[] productIds) => new("StockNotAvailable", $"Stock not available for product(s): {string.Join(", ", productIds)}");

    internal static object StockNotAvailable(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}

