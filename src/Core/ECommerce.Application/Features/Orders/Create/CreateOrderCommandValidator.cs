using FluentValidation;
using ECommerce.Application.Features.Orders.Validators;

namespace ECommerce.Application.Features.Orders.Create;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IStockService _stockService;
    public CreateOrderCommandValidator(IStockService stockService)
    {
        _stockService = stockService;

        RuleFor(x => x)
            .MustAsync(async (x, token) => !(await ValidateProductStock(x, token)).Any())
            .WithMessage(x => OrderErrors.StockNotAvailable(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage(OrderErrors.InvalidPaymentMethod.Message);

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator());
    }

    private async Task<List<Guid>> ValidateProductStock(CreateOrderCommand command, CancellationToken token)
    {
        List<Guid> productIds = [];
        foreach (var orderItem in command.OrderItems)
        {
            if (!await _stockService.IsStockAvailable(orderItem.ProductId, orderItem.Quantity, token))
                productIds.Add(orderItem.ProductId);
        }
        return productIds;
    }
}
