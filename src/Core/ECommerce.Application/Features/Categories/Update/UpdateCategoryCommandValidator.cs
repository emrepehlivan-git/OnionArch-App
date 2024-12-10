using ECommerce.Application.Repositories;
using FluentValidation;

namespace ECommerce.Application.Features.Categories.Update
{
    public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(c => c.Id)
                .MustAsync(IsCategoryExistsAsync)
                .WithMessage(CategoryErrors.CategoryNotFound.Message);

            RuleFor(c => c.Name)
                .MinimumLength(CategoryConsts.NameMinLength)
                .WithMessage(CategoryErrors.CategoryNameMinLength.Message)
                .MaximumLength(CategoryConsts.NameMaxLength)
                .WithMessage(CategoryErrors.CategoryNameMaxLength.Message);

            RuleFor(c => c)
                .MustAsync(IsUniqueNameAsync)
                .WithMessage(CategoryErrors.CategoryNameMustBeUnique.Message);
        }
        private async Task<bool> IsUniqueNameAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            return !await _categoryRepository.AnyAsync(c => c.Id != command.Id && c.Name.ToLower() == command.Name.Trim().ToLower(), cancellationToken);
        }

        private async Task<bool> IsCategoryExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.AnyAsync(c => c.Id == id, cancellationToken);
        }
    }
}