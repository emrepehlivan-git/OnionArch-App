using ECommerce.Application.Behaviors.Validation;
using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Features.Orders.Create;

public record CreateOrderCommand(
    Guid CustomerId,
    List<OrderItemDto> OrderItems,
    PaymentMethod PaymentMethod
) : IRequest<Result<Guid>>, IValidetableRequest;
