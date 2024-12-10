using ECommerce.Application.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Categories.GetById;

public sealed class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetByIdQueryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(query => query.Id)
            .NotEmpty()
            .MustAsync(async (id, cancellationToken) =>
                await _categoryRepository.AnyAsync(category => category.Id == id, cancellationToken))
                .WithMessage("Category not found");
    }
}

