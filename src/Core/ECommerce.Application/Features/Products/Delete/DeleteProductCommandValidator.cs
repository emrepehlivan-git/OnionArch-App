using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Products.Delete;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x.Id)
            .MustAsync(ProductMustExistAsync)
            .WithMessage(ProductErrors.ProductNotFound.Message);
    }

    private async Task<bool> ProductMustExistAsync(Guid id, CancellationToken cancellationToken)
        => await _productRepository.AnyAsync(x => x.Id == id, cancellationToken);
}
