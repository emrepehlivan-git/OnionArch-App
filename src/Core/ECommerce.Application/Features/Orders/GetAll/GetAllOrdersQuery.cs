using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Orders.GetAll;

public record GetAllOrdersQuery(PaginationParams Filter) : IRequest<PaginatedResult<OrderDto>>;
