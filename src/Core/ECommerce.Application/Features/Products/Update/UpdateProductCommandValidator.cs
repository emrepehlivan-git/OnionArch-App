
using ECommerce.Application.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Products.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandValidator(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository
)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;

        RuleFor(x => x.Id)
            .MustAsync(ProductExistsAsync)
            .WithMessage(ProductErrors.ProductNotFound.Message);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ProductErrors.ProductNameNotEmpty.Message)
            .MinimumLength(ProductConsts.NameMinLength)
            .WithMessage(ProductErrors.ProductNameMinLength.Message)
            .MaximumLength(ProductConsts.NameMaxLength)
            .WithMessage(ProductErrors.ProductNameMaxLength.Message);

        RuleFor(x => x.Description)
            .MaximumLength(ProductConsts.DescriptionMaxLength)
            .WithMessage(ProductErrors.ProductDescriptionMaxLength.Message);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(ProductConsts.MinPrice)
            .WithMessage(ProductErrors.ProductPriceMustBeGreaterThanZero.Message);

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExistsAsync)
            .WithMessage(ProductErrors.CategoryNotFound.Message);
    }

    private async Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _categoryRepository.AnyAsync(x => x.Id == categoryId, cancellationToken);
    }

    private async Task<bool> ProductExistsAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _productRepository.AnyAsync(x => x.Id == productId, cancellationToken);
    }
}
