using System.Globalization;
using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Application.Features.Orders.GetAll;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;
using Mapster;
namespace ECommerce.ApplicationTest.Orders.GetAll;

public class GetAllOrdersQueryHandlerTests : OrderTestBase
{
    private readonly GetAllOrdersQueryHandler _handler;
    private readonly PaginationParams _paginationParams;
    private readonly GetAllOrdersQuery _query;

    public GetAllOrdersQueryHandlerTests()
    {
        _handler = new GetAllOrdersQueryHandler(OrderRepositoryMock.Object);
        _paginationParams = new PaginationParams(1, 10);
        _query = new GetAllOrdersQuery(_paginationParams);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllOrders()
    {
        var orders = new List<Domain.Entities.Order>
        {
            Domain.Entities.Order.Create(PaymentMethod.BankTransfer, new Address("123 Main St", "City", "State", "12345", "12345")),
            Domain.Entities.Order.Create(PaymentMethod.CreditCard, new Address("456 Elm St", "City", "State", "12345", "12345"))
        };

        var paginatedResult = new PaginatedResult<Order>(orders, _paginationParams.PageNumber, _paginationParams.PageSize, orders.Count, orders.Count);

        OrderRepositoryMock.Setup(repo => repo.GetAllAsync(
        _paginationParams,
        It.IsAny<Expression<Func<Order, bool>>>(),
        It.IsAny<Expression<Func<Order, string>>[]>(),
        false,
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(paginatedResult);
        var orderDtos = paginatedResult.Items.Adapt<List<OrderDto>>();

        var result = await _handler.Handle(_query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(orders.Count);
        result.PageNumber.Should().Be(_paginationParams.PageNumber);
        result.PageSize.Should().Be(_paginationParams.PageSize);
        result.TotalCount.Should().Be(orders.Count);
    }
}
