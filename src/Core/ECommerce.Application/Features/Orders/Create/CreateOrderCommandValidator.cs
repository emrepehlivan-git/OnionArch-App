using FluentValidation;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.ValueObjects;
using ECommerce.Application.Features.Orders.Validators;

namespace ECommerce.Application.Features.Orders.Create;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IStockService _stockService;
    public CreateOrderCommandValidator(IStockService stockService)
    {
        _stockService = stockService;

        RuleFor(x => x)
            .MustAsync(ValidateProductStock)
            .WithMessage(x => OrderErrors.OneOrMoreOrderItemsNotInStock(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage(OrderErrors.InvalidPaymentMethod.Message);

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator());
    }

    private async Task<bool> ValidateProductStock(CreateOrderCommand command, CancellationToken token)
    {
        foreach (var orderItem in command.OrderItems)
            if (!await _stockService.IsStockAvailable(orderItem.ProductId, orderItem.Quantity))
                return false;

        return true;
    }
}
