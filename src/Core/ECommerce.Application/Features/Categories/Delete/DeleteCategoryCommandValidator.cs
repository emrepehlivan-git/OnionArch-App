using ECommerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Categories.Delete
{
    public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(c => c.Id)
            .MustAsync(async (id, cancellationToken) => await _categoryRepository.AnyAsync(c => c.Id == id, cancellationToken))
            .WithMessage("Category not found");
        }
    }
}