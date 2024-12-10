using ECommerce.Application.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Categories.Create
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(c => c.Name)
            .NotEmpty().NotNull()
            .MinimumLength(CategoryConsts.NameMinLength)
            .WithMessage(CategoryErrors.CategoryNameMinLength.Message)
            .MaximumLength(CategoryConsts.NameMaxLength)
            .WithMessage(CategoryErrors.CategoryNameMaxLength.Message)
            .MustAsync(IsUniqueNameAsync)
            .WithMessage(CategoryErrors.CategoryNameMustBeUnique.Message);
        }
        private async Task<bool> IsUniqueNameAsync(string name, CancellationToken cancellationToken)
        {
            return !await _categoryRepository.AnyAsync(c => c.Name.ToLower() == name.Trim().ToLower(), cancellationToken);
        }
    }
}
