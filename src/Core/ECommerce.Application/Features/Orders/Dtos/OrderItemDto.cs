namespace ECommerce.Application.Features.Orders.Dtos;

public record OrderItemDto(Guid Id, Guid ProductId, decimal Price, int Quantity);
