using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Categories.GetById;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(query => query.Id)
            .NotEmpty()
            .MustAsync(async (id, cancellationToken) =>
                await _categoryRepository.AnyAsync(category => category.Id == id, cancellationToken))
                .WithMessage("Category not found");
    }
}

