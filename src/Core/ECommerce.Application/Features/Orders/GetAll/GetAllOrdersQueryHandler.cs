using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Orders.GetAll;

public sealed class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetAllOrdersQuery, PaginatedResult<OrderDto>>
{
    public async Task<PaginatedResult<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await orderRepository.GetAllAsync(request.Filter, cancellationToken: cancellationToken);
        var dtos = result.Items.Adapt<List<OrderDto>>();
        return new(dtos, request.Filter.PageNumber, request.Filter.PageSize, result.TotalPages, result.TotalCount);
    }
}
