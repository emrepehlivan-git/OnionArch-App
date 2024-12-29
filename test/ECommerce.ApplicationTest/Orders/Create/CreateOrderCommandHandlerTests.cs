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
    private readonly Guid customerId = new Guid("3409c3e3-c9f5-4d66-a9e2-d548075ed7cd");
    private readonly PaymentMethod _paymentMethod = PaymentMethod.CreditCard;
    private readonly Address _address = new Address("Turkey", "123 Main St", "Istanbul", "34720", "34720");

    public CreateOrderCommandHandlerTests()
    {
        _handler = new CreateOrderCommandHandler(OrderRepositoryMock.Object, OrderItemRepositoryMock.Object);
        _command = new CreateOrderCommand(customerId, [new OrderItemDto(Guid.NewGuid(), 1, 1)], _paymentMethod, _address);
        _validator = new CreateOrderCommandValidator(StockServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder_WhenCommandIsValid()
    {
        var order = Order.Create(_command.PaymentMethod, _command.Address);
        OrderRepositoryMock.Setup(x => x.AddAsync(order, CancellationToken.None))
        .ReturnsAsync(order);
        OrderItemRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<List<OrderItem>>(), CancellationToken.None)).ReturnsAsync(new List<OrderItem>());
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
    public async Task Handle_ShouldReturnFailureResult_WhenCommandIsAddressIsInvalid()
    {
        _command = _command with { Address = new Address("", "", "", "", "") };
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.InvalidAddress.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductStockIsUnavailable()
    {
        // Arrange
        var unavailableProductId = Guid.NewGuid();
        _command = _command with
        {
            OrderItems = new List<OrderItemDto>
        {
            new OrderItemDto(unavailableProductId, 1, 1)
        }
        };

        StockServiceMock.Setup(x => x.IsStockAvailable(unavailableProductId, 1, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.StockNotAvailable(new[] { unavailableProductId }).Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductStockIsAvailable()
    {
        // Arrange
        var availableProductId = Guid.NewGuid();
        _command = _command with
        {
            OrderItems = new List<OrderItemDto>
        {
            new OrderItemDto(availableProductId, 1, 1)
        }
        };

        StockServiceMock.Setup(x => x.IsStockAvailable(availableProductId, 1, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(_command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
