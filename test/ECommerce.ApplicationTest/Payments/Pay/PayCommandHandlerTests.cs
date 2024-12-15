using ECommerce.Application.Features.Orders;
using ECommerce.Application.Features.Payments;
using ECommerce.Application.Features.Payments.Pay;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Entities;

namespace ECommerce.ApplicationTest.Payments.Pay;

public sealed class PayCommandHandlerTests : PaymentsTestBase
{
    private readonly PayCommandHandler _handler;
    private readonly PayCommandValidator _validator;

    public PayCommandHandlerTests()
    {
        _handler = new(PaymentServiceMock.Object);
        _validator = new(OrderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        var command = new PayCommand(PaymentRequest);
        PaymentServiceMock
            .Setup(x => x.ProcessPaymentAsync(PaymentRequest, CancellationToken.None))
            .ReturnsAsync(Result.Success());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsPaymentFailed()
    {
        var command = new PayCommand(PaymentRequest);
        // Given
        PaymentServiceMock
            .Setup(x => x.ProcessPaymentAsync(PaymentRequest, CancellationToken.None))
            .ReturnsAsync(Result.Failure(PaymentErrors.PaymentFailed));

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PaymentErrors.PaymentFailed);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsOrderNotFound()
    {
        var command = new PayCommand(PaymentRequest);
        OrderRepositoryMock
            .Setup(x => x.GetByIdAsync(OrderId, CancellationToken.None))
            .ReturnsAsync((Order)null!);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == OrderErrors.OrderNotFound.Message);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsCardNumberIsRequired()
    {
        var paymentRequest = PaymentRequest with { CardNumber = string.Empty };
        var command = new PayCommand(paymentRequest);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == PaymentErrors.CardNumberRequired.Message);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsCardHolderNameIsRequired()
    {
        var paymentRequest = PaymentRequest with { CardHolderName = string.Empty };
        var command = new PayCommand(paymentRequest);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == PaymentErrors.CardHolderNameRequired.Message);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsCardExpirationDateIsRequired()
    {
        var paymentRequest = PaymentRequest with { ExpiryDate = string.Empty };
        var command = new PayCommand(paymentRequest);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == PaymentErrors.ExpiryDateRequired.Message);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsCardSecurityCodeIsRequired()
    {
        var paymentRequest = PaymentRequest with { Cvv = string.Empty };
        var command = new PayCommand(paymentRequest);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == PaymentErrors.SecurityCodeRequired.Message);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsCardAmountIsRequired()
    {
        var paymentRequest = PaymentRequest with { Amount = 0 };
        var command = new PayCommand(paymentRequest);
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == PaymentErrors.AmountMustBeGreaterThanZero.Message);
    }
}
