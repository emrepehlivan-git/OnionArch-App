namespace ECommerce.Application.Features.Orders.Dtos;

public record OrderItemDto(Guid ProductId, decimal Price, int Quantity);
