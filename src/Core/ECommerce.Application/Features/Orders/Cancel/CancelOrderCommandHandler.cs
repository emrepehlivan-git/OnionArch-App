using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Features.Orders.Cancel;

public sealed class CancelOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        order!.UpdateOrderStatus(OrderStatus.Cancelled);
        orderRepository.Update(order);
        return Result.Success();
    }
}
