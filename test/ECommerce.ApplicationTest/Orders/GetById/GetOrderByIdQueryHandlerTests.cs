using System;
using ECommerce.Application.Features.Orders;
using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Features.Orders.GetById;
using ECommerce.Domain.Entities;
using Mapster;
namespace ECommerce.ApplicationTest.Orders.GetById;

public class GetOrderByIdQueryHandlerTests : OrderTestBase
{
    private readonly GetOrderByIdQueryHandler _handler;
    private readonly GetOrderByIdQueryValidator _validator;
    public GetOrderByIdQueryHandlerTests()
    {
        _handler = new GetOrderByIdQueryHandler(OrderRepositoryMock.Object);
        _validator = new GetOrderByIdQueryValidator(OrderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrderDto_WhenOrderExists()
    {
        OrderRepositoryMock.Setup(x => x.GetByIdAsync(DefaultOrder.Id, CancellationToken.None)).ReturnsAsync(DefaultOrder);
        var order = await _handler.Handle(new GetOrderByIdQuery(DefaultOrder.Id), CancellationToken.None);

        order.IsSuccess.Should().BeTrue();
        order.Value.Should().Be(DefaultOrder.Adapt<OrderDto>());
    }

    [Fact]
    public async Task Handle_ShouldReturnOrderNotFoundError_WhenOrderDoesNotExist()
    {
        OrderRepositoryMock.Setup(x => x.GetByIdAsync(Guid.NewGuid(), CancellationToken.None)).ReturnsAsync((Order)null!);
        var result = await _handler.Handle(new GetOrderByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(OrderErrors.OrderNotFound);
        result.Value.Should().Be(result.Value.Adapt<OrderDto>());
    }
}
