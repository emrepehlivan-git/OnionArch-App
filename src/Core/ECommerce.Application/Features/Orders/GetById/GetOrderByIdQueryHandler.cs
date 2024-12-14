using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;
using ECommerce.Application.Interfaces.Repositories;
using Mapster;
namespace ECommerce.Application.Features.Orders.GetById;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        return order is null ? Result<OrderDto>.Failure(OrderErrors.OrderNotFound) : Result<OrderDto>.Success(order.Adapt<OrderDto>());
    }
}
