using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Orders.Cancel;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrors.OrderNotFound.Message)
            .MustAsync(OrderExists)
            .WithMessage(OrderErrors.OrderNotFound.Message);
    }

    private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
    {
        return await _orderRepository.AnyAsync(x => x.Id == orderId, cancellationToken);
    }
}
