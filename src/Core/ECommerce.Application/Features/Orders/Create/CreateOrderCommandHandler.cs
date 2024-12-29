using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.DomainEvents.Orders;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Create;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IOrderItemRepository orderItemRepository
) : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.PaymentMethod, request.Address);

        List<OrderItem> orderItems = [];
        foreach (var item in request.OrderItems)
        {
            var orderItem = OrderItem.Create(
                item.ProductId,
                order.Id,
                item.Price,
                item.Quantity
            );
            orderItems.Add(orderItem);
        }

        order.AddItems(orderItems);
        await Task.WhenAll(
            orderRepository.AddAsync(order, cancellationToken),
            orderItemRepository.AddRangeAsync(orderItems, cancellationToken)
        );
        return Result<Guid>.Success(order.Id);
    }
}
