using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Orders;

public static class OrderErrors
{
    public static Error OrderNotFound => new("OrderNotFound", "Order not found");
    public static Error OrderCreationFailed => new("OrderCreationFailed", "Order creation failed");
    public static Error OrderItemNotFound(Guid orderItemId) => new("OrderItemNotFound", $"Order item with id {orderItemId} not found");
    public static Error OrderItemAlreadyExists(Guid orderItemId) => new("OrderItemAlreadyExists", $"Order item with id {orderItemId} already exists");
    public static Error OneOrMoreOrderItemsNotFound(Guid[] productIds) => new("OneOrMoreOrderItemsNotFound", $"One or more order items not found: {string.Join(", ", productIds)}");
    public static Error OneOrMoreOrderItemsNotInStock(Guid[] productIds) => new("OneOrMoreOrderItemsNotInStock", $"One or more order items not in stock: {string.Join(", ", productIds)}");
    public static Error InvalidPaymentMethod => new("InvalidPaymentMethod", "Invalid payment method");
    public static Error InvalidAddress => new("InvalidAddress", "All fields are required");
}

