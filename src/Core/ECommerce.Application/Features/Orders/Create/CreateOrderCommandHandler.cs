using ECommerce.Application.Features.Products;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Create;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IOrderItemRepository orderItemRepository,
    IProductRepository productRepository,
    IStockService stockService
) : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var products = productRepository
            .GetByCondition(product => request.OrderItems.Select(x => x.ProductId).Contains(product.Id))
            .ToList();

        var order = Order.Create(request.PaymentMethod, request.Address);

        List<OrderItem> orderItems = [];
        foreach (var item in request.OrderItems)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var orderItem = OrderItem.Create(
                order.Id,
                product.Id,
                product.Price,
                item.Quantity
            );
            orderItems.Add(orderItem);

            if (!await stockService.ReserveStock(product.Id, item.Quantity))
                return Result<Guid>.Failure(ProductErrors.ProductStockNotEnough(product.Id, product.Stock));
        }

        order.AddItems(orderItems);
        productRepository.UpdateRange(products);
        await orderRepository.AddAsync(order, cancellationToken);
        await orderItemRepository.AddRangeAsync(orderItems, cancellationToken);
        return Result<Guid>.Success(order.Id);
    }
}
