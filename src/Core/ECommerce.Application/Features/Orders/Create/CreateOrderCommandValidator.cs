using FluentValidation;
using ECommerce.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Features.Orders.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x)
            .MustAsync(ValidateProducts)
            .WithMessage(x => OrderErrors.OneOrMoreOrderItemsNotFound(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x)
            .MustAsync(ValidateProductStock)
            .WithMessage(x => OrderErrors.OneOrMoreOrderItemsNotInStock(x.OrderItems.Select(y => y.ProductId).ToArray()).Message);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage(OrderErrors.InvalidPaymentMethod.Message);
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
            if (!await _productRepository.IsProductInStock(orderItem.ProductId, orderItem.Quantity))
                return false;

        return true;
    }
}
