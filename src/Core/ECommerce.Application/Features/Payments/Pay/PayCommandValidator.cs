using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Payments.Pay;

public sealed class PayCommandValidator : AbstractValidator<PayCommand>
{
    private readonly IOrderRepository _orderRepository;

    public PayCommandValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(payment => payment.PaymentRequest.OrderId)
            .MustAsync(OrderExists)
            .WithMessage(PaymentErrors.OrderNotFound.Message);

        RuleFor(payment => payment.PaymentRequest.CardNumber)
            .NotEmpty()
            .WithMessage(PaymentErrors.CardNumberRequired.Message);

        RuleFor(payment => payment.PaymentRequest.CardHolderName)
            .NotEmpty()
            .WithMessage(PaymentErrors.CardHolderNameRequired.Message);

        RuleFor(payment => payment.PaymentRequest.ExpiryDate)
            .NotEmpty()
            .WithMessage(PaymentErrors.ExpiryDateRequired.Message);

        RuleFor(payment => payment.PaymentRequest.Amount)
            .Must(amount => amount > 0)
            .WithMessage(PaymentErrors.AmountMustBeGreaterThanZero.Message);
    }

    public async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
    {
        return await _orderRepository.AnyAsync(order => order.Id == orderId, cancellationToken);
    }
}
