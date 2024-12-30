using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.Dtos;

public record OrderDto(Guid Id, string OrderNumber, decimal TotalAmount, DateTime OrderDate, OrderStatus Status);
