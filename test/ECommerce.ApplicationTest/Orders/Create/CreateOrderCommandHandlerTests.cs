using ECommerce.Application.Features.Orders;
using ECommerce.Application.Features.Orders.Create;
using ECommerce.Application.Features.Orders.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.ApplicationTest.Orders.Create;

public class CreateOrderCommandHandlerTests : OrderTestBase
{
    private readonly CreateOrderCommandHandler _handler;
    private CreateOrderCommand _command;
    private readonly CreateOrderCommandValidator _validator;
    private readonly Guid customerId = Guid.NewGuid();
    private readonly List<OrderItemDto> _orderItems = [];
    private readonly PaymentMethod _paymentMethod = PaymentMethod.CreditCard;
    private readonly Address _address = new Address("Turkey", "123 Main St", "Istanbul", "34720", "34720");

    public CreateOrderCommandHandlerTests()
    {
        _handler = new CreateOrderCommandHandler(OrderRepositoryMock.Object, OrderItemRepositoryMock.Object, ProductRepositoryMock.Object);
        _command = new CreateOrderCommand(customerId, _orderItems, _paymentMethod, _address);
        _validator = new CreateOrderCommandValidator(ProductRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder_WhenCommandIsValid()
    {
        var order = Order.Create(_command.PaymentMethod, _command.Address);
        OrderRepositoryMock.Setup(x => x.AddAsync(order, CancellationToken.None))
        .ReturnsAsync(order);
        OrderItemRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<List<OrderItem>>(), CancellationToken.None)).ReturnsAsync(new List<OrderItem>());
        ProductRepositoryMock.Setup(x => x.UpdateRange(It.IsAny<List<Product>>())).Returns(new List<Product>());
        var result = await _handler.Handle(_command, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCommandIsPaymentMethodIsInvalid()
    {
        _command = _command with { PaymentMethod = (PaymentMethod)6 };
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.InvalidPaymentMethod.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCommandIsOrderItemsAreNotInStock()
    {
        _command = _command with { OrderItems = [new OrderItemDto(Guid.NewGuid(), Guid.NewGuid(), 1, 1)] };
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.OneOrMoreOrderItemsNotInStock(new[] { _command.OrderItems[0].ProductId }).Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCommandIsOrderItemsAreNotFound()
    {
        _command = _command with { OrderItems = [new OrderItemDto(Guid.NewGuid(), Guid.NewGuid(), 1, 1)] };
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.OneOrMoreOrderItemsNotFound(new[] { _command.OrderItems[0].ProductId }).Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCommandIsAddressIsInvalid()
    {
        _command = _command with { Address = new Address("", "", "", "", "") };
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.InvalidAddress.Message);
    }
}
