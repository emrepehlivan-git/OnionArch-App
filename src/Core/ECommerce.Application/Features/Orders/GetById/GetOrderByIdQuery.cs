using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Orders.GetById;

public record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderDto>>;
