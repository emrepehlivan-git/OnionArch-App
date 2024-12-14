using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Orders.GetById;

public sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrderByIdQueryValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(x => x.Id)
        .NotEmpty()
        .MustAsync(async (id, cancellationToken)
            => await _orderRepository.AnyAsync(x => x.Id == id, cancellationToken))
            .WithMessage(OrderErrors.OrderNotFound.Message);
    }
}
