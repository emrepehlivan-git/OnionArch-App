using FluentValidation;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Application.Features.Orders.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IStockService _stockService;
    public CreateOrderCommandValidator(IProductRepository productRepository, IStockService stockService)
    {
        _productRepository = productRepository;
        _stockService = stockService;

        RuleFor(x => x)
            .MustAsync(ValidateProducts)
            .WithMessage(x => OrderErrors.OneOrMoreOrderItemsNotFound(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x)
            .MustAsync(ValidateProductStock)
            .WithMessage(x => OrderErrors.OneOrMoreOrderItemsNotInStock(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage(OrderErrors.InvalidPaymentMethod.Message);

        RuleFor(x => x.Address)
            .Must(IsValidAddress)
            .WithMessage(OrderErrors.InvalidAddress.Message);
    }

    private async Task<bool> ValidateProducts(CreateOrderCommand command, CancellationToken token)
    {
        var productIds = command.OrderItems.Select(x => x.ProductId).ToArray();

        return await Task.FromResult(_productRepository
            .GetByCondition(x => productIds.Contains(x.Id))
            .Count() == productIds.Length);
    }

    private async Task<bool> ValidateProductStock(CreateOrderCommand command, CancellationToken token)
    {
        foreach (var orderItem in command.OrderItems)
            if (!await _stockService.IsStockAvailable(orderItem.ProductId, orderItem.Quantity))
                return false;

        return true;
    }

    private static bool IsValidAddress(Address address)
    {
        return !string.IsNullOrEmpty(address.Country) &&
               !string.IsNullOrEmpty(address.Street) &&
               !string.IsNullOrEmpty(address.City) &&
               !string.IsNullOrEmpty(address.State) &&
               !string.IsNullOrEmpty(address.ZipCode);
    }
}
