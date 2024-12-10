using ECommerce.Application.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Products.Create;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

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
}