using ECommerce.Application.Features.Orders;
using ECommerce.Application.Features.Orders.Cancel;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.ApplicationTest.Orders.Cancel;

public class CancelOrderCommanHandlerTests : OrderTestBase
{
    private readonly CancelOrderCommandHandler _handler;
    private readonly CancelOrderCommandValidator _validator;
    private readonly CancelOrderCommand _command;
    private readonly Guid _orderId;

    public CancelOrderCommanHandlerTests()
    {
        _handler = new CancelOrderCommandHandler(OrderRepositoryMock.Object);
        _validator = new CancelOrderCommandValidator(OrderRepositoryMock.Object);
        _orderId = Guid.NewGuid();
        _command = new CancelOrderCommand(_orderId);
    }

    [Fact]
    public async Task Handle_OrderCancelled_ReturnsSuccess()
    {
        // Arrange
        var command = new CancelOrderCommand(_orderId);
        var address = new Address("Street", "City", "State", "Country", "ZipCode");
        var order = Order.Create(PaymentMethod.CreditCard, address);
        OrderRepositoryMock.Setup(x => x.GetByIdAsync(_orderId, CancellationToken.None)).ReturnsAsync(order);
        order.UpdateOrderStatus(OrderStatus.Cancelled);
        OrderRepositoryMock.Setup(x => x.Update(order));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new CancelOrderCommand(Guid.NewGuid());
        OrderRepositoryMock.Setup(x => x.GetByIdAsync(command.OrderId, CancellationToken.None))
        .ReturnsAsync((Order?)null);
        // Act
        var result = await _validator.ValidateAsync(command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().ErrorMessage.Should().Be(OrderErrors.OrderNotFound.Message);
    }
}
